﻿using Airbnb.Application.Features.Notifications;
using Airbnb.Application.Settings;
using Airbnb.Application.Utility;
using Airbnb.Domain;
using Airbnb.Domain.DataTransferObjects.Property;
using Airbnb.Domain.Entities;
using Airbnb.Domain.Identity;
using Airbnb.Domain.Interfaces.Repositories;
using Airbnb.Domain.Interfaces.Services;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace Airbnb.Application.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMediator _mediator;
		public PropertyService(IUnitOfWork unitOfWork,
			UserManager<AppUser> userManager,
			IHttpContextAccessor contextAccessor,
			IMapper mapper,
			IPublisher publisher,
			IMediator mediator)

		{
			_unitOfWork = unitOfWork;
			_userManager = userManager;
			_contextAccessor = contextAccessor;
			_mapper = mapper;
			_mediator = mediator;
		}
		public async Task<Responses> CreatePropertyAsync(PropertyRequest propertyDTO)
        {
            var owner = await GetUser.GetCurrentUserAsync(_contextAccessor,_userManager);
            var user = await _userManager.FindByEmailAsync(propertyDTO.OwnereEmail);
            if (owner == null || user == null || user.Email != owner.Email)
            {
                return await Responses.FailurResponse("Unauthenticated request!", HttpStatusCode.Unauthorized);
            }

            var Location = await _unitOfWork.Repository<Location, int>().GetByNameAsync(propertyDTO.Location!)!;
            if (Location == null)
            {
                return await Responses.FailurResponse("Location InValid!", HttpStatusCode.BadRequest);
            }

            var country = await _unitOfWork.Repository<Country, int>().GetByNameAsync(propertyDTO.Country!)!;
            if (country == null || Location.CountryId != country.Id)
            {
                return await Responses.FailurResponse("Country InValid!", HttpStatusCode.BadRequest);
            }

            var Region = await _unitOfWork.Repository<Region, int>().GetByNameAsync(propertyDTO.Region!)!;
            if (Region == null || country.RegionId != Region.Id)
            {
                return await Responses.FailurResponse("Region InValid!", HttpStatusCode.BadRequest);
            }

            var categories = new List<Category>();
            foreach (var i in propertyDTO.Categories!)
            {
                var category = await _unitOfWork.Repository<Category, int>().GetByNameAsync(i)!;
                if (category is not null)
                {
                    categories.Add(category);
                }
            }

            if (!categories.Any())
            {
                return await Responses.FailurResponse("There is no categories valid!", HttpStatusCode.BadRequest);
            }

            var images = new List<Image>();
            foreach (var img in propertyDTO.Images!)
            {
                if (img != null)
                {
                    var obj = new Image()
                    {
                        Name = await DocumentSettings.UploadFile(img, SD.Image, SD.Property)
                    };
                    images.Add(obj);
                }

            }

            try
            {
                string PropertyId = Guid.NewGuid().ToString();



                var roomServices = new List<RoomService>();

                foreach (var name in propertyDTO.RoomServices!)
                {
                    if (name != null)
                    {
                        roomServices.Add(new RoomService()
                        {
                            Name = name
                        });
                    }
                }

                var createdProperty = new Property()
                {
                    Id = PropertyId,
                    Name = propertyDTO.Description!,
                    Description = propertyDTO.Description!,
                    Rate = 0,
                    Bookings = new List<Booking>(),
                    Reviews = new List<Review>(),
                    NightPrice = (decimal)propertyDTO.NightPrice!,
                    PlaceType = propertyDTO.PlaceType!,
                    LocationId = Location.Id,
                    OwnerId = owner.Id,
                    Images = images,
                    Categories = categories,
                    RoomServices = roomServices
                };
                await _unitOfWork.Repository<Property, string>().AddAsync(createdProperty);
                await _unitOfWork.CompleteAsync();

				await _mediator.Publish(new NotificationEvent()
				{
					Message = "Your Property has been added successfully!",
					UserId = owner.Id,
					IsPublic = false
				});

				return await Responses.SuccessResponse("Property has been created successfully!");
            }
            catch (Exception ex)
            {
                foreach (var i in images)
                {
                    var sub = i.Name.Split('/');
                    if (sub.Length == 4)
                    {
                        await DocumentSettings.DeleteFile(sub[1], sub[3], sub[3]);
                    }
                }
                return await Responses.FailurResponse(ex, HttpStatusCode.InternalServerError);
            }


        }
        public async Task<Responses> DeletePropertyAsync(string propertyId)
        {
            var user = await GetUser.GetCurrentUserAsync(_contextAccessor, _userManager);
            if (user == null)
            {
                return await Responses.FailurResponse("Unauthenticated request!", HttpStatusCode.Unauthorized);
            }

            var property = await _unitOfWork.Repository<Property, string>().GetByIdAsync(propertyId)!;
            if (property == null) return await Responses.FailurResponse("There is no property with this id");

            if (property.OwnerId != user.Id)
            {
                return await Responses.FailurResponse("Unauthenticated request!", HttpStatusCode.Unauthorized);
            }
            if (property.Bookings.Any())
            {
                return await Responses.FailurResponse("You can't remove you property, It's already booked!", HttpStatusCode.Unauthorized);
            }
            try
            {
                _unitOfWork.Repository<Property, string>().Remove(property);
                var Images = property.Images;
                foreach (var i in Images)
                {
                    var splitted = i.Name.Split('/');
                    if (splitted.Length == 4)
                    {
                        await DocumentSettings.DeleteFile(splitted[1], splitted[2], splitted[3]);
                    }
                }
                await _unitOfWork.CompleteAsync();
				await _mediator.Publish(new NotificationEvent()
				{
					Message = "Property Deleted Successfully",
					UserId = property.OwnerId,
					IsPublic = false
				});
				return await Responses.SuccessResponse("Property has been deleted successfully!");
            }
            catch (Exception ex)
            {
                return await Responses.FailurResponse(ex.Message, HttpStatusCode.BadRequest);
            }

        }
        public async Task<Responses> GetAllPropertiesAsync()
        {
            // there is a cycle when return the object
            var properties = await _unitOfWork.Repository<Property, string>().GetAllAsync();

            if (!properties.Any()) return await Responses.FailurResponse("There is no properties found", HttpStatusCode.NotFound);

            return await Responses.SuccessResponse(_mapper.Map<IEnumerable<PropertyResponse>>(properties));
        }
        public async Task<Responses> GetPropertyByIdAsync(string propertyId)
        {
            var user = await GetUser.GetCurrentUserAsync(_contextAccessor, _userManager);
            if (user == null)
            {
                return await Responses.FailurResponse("Unauthenticated request!", HttpStatusCode.Unauthorized);
            }

            var property = await _unitOfWork.Repository<Property, string>().GetByIdAsync(propertyId)!;

            if (property == null) return await Responses.FailurResponse("InValid propertyId ", HttpStatusCode.NotFound);

            if (property.OwnerId != user.Id)
            {
                return await Responses.FailurResponse("UnAuthorized!", HttpStatusCode.Unauthorized);
            }

            return await Responses.SuccessResponse(_mapper.Map<PropertyResponse>(property));
        }
        public async Task<Responses> UpdatePropertyAsync(UpdatePropertyDto propertyDTO)
        {
            var user = await GetUser.GetCurrentUserAsync(_contextAccessor, _userManager);
            if (user == null)
            {
                return await Responses.FailurResponse("Unauthenticated request!", HttpStatusCode.Unauthorized);
            }

            var property = await _unitOfWork.Repository<Property, string>().GetByIdAsync(propertyDTO.PropertyId)!;

            if (property == null) return await Responses.FailurResponse("Property is not found!", HttpStatusCode.NotFound);

            if (property.OwnerId != user.Id)
            {
                return await Responses.FailurResponse("Unauthenticated request!", HttpStatusCode.Unauthorized);
            }
            try
            {

                property.Name = propertyDTO.Name != null ? propertyDTO.Name : property.Name;
                property.Description = propertyDTO.Description != null ? propertyDTO.Description : property.Description;
                property.NightPrice =propertyDTO.NightPrice != null ? (decimal)propertyDTO.NightPrice : property.NightPrice;
                property.PlaceType = propertyDTO.PlaceType != null ? propertyDTO.PlaceType : property.PlaceType;
                property.RoomServices = propertyDTO.RoomServices != null ?
                    propertyDTO.RoomServices
                    .Select(room => new RoomService()
                    {
                        PropertyId = property.Id,
                        Name = room
                    }).ToList() : property.RoomServices;


                _unitOfWork.Repository<Property, string>().Update(property);
                await _unitOfWork.CompleteAsync();

				await _mediator.Publish(new NotificationEvent()
				{
					Message = "See new updates!",
					IsPublic = true
				});

				return await Responses.SuccessResponse("Property has been updated successfully!");
            }
            catch (Exception ex)
            {

                return await Responses.FailurResponse(ex.Message.ToList(), HttpStatusCode.InternalServerError);
            }

        }
    }
}
