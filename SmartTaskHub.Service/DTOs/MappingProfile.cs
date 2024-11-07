using AutoMapper;
using SmartTaskHub.Core.Entity;
using SmartTaskHub.Infrastructure.Redis;
using SmartTaskHub.Service.DTOs.Requests;

namespace SmartTaskHub.Service.DTOs;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        CreateMap<CreateTaskTimeoutRuleRequest, TaskTimeoutRule>();
        CreateMap<TaskTimeoutRule,TaskTimeoutRuleDto>();
    
        CreateMap<CreateEquipMaintOrderRequest, EquipMaintOrder>()
            .ForMember(dest => dest.PlannedCompletionTime, opt => opt.Ignore());;
        CreateMap<EquipMaintOrder,EquipMaintOrderDto>();

        CreateMap<TaskDetailsDto, TaskDetails>();
    }
}