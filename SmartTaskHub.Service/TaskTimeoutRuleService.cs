using AutoMapper;
using SmartTaskHub.Core.Entity;
using SmartTaskHub.Core.Interface;
using SmartTaskHub.Service.DTOs;
using SmartTaskHub.Service.DTOs.Requests;
using SmartTaskHub.Service.Interfaces;

namespace SmartTaskHub.Service;

public class TaskTimeoutRuleService : ITaskTimeoutRuleService
{
    private readonly ITaskTimeoutRuleRepository _taskTimeoutRuleRepository;
    
    private readonly IMapper _mapper;
    
    public TaskTimeoutRuleService(ITaskTimeoutRuleRepository taskTimeoutRuleRepository, IMapper mapper)
    {
        _taskTimeoutRuleRepository = taskTimeoutRuleRepository;
        _mapper = mapper;
    }
    
    /// <summary>
    /// 添加任务超时规则
    /// </summary>
    /// <param name="request">request</param>
    public async Task<long> AddTaskTimeoutRuleAsync(CreateTaskTimeoutRuleRequest request)
    {
        try
        {
            var taskTimeoutRule = _mapper.Map<TaskTimeoutRule>(request);
            taskTimeoutRule.TimeoutDuration = TimeSpan.FromMinutes(request.TimeoutDurationInMinutes);
            await _taskTimeoutRuleRepository.AddAsync(taskTimeoutRule);

            return taskTimeoutRule.Id;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    /// <summary>
    /// 删除任务超时规则
    /// </summary>
    /// <param name="id">id</param>
    public async Task DeleteTaskTimeoutRuleAsync(long id)
    {
        try
        {
            
            var taskTimeoutRule = await _taskTimeoutRuleRepository.GetByIdAsync(id);
            if (taskTimeoutRule == null)
            {
                throw new Exception($"未找到 {id} 任务超时规则");
            }
            _taskTimeoutRuleRepository.Delete(taskTimeoutRule);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    /// <summary>
    /// 查询任务超时规则
    /// </summary>
    /// <param name="id">id</param>
    /// <returns></returns>
    public async Task<TaskTimeoutRuleDto> GetTaskTimeoutRuleAsync(long id)
    {
        var taskTimeoutRule = await _taskTimeoutRuleRepository.GetByIdAsync(id);
        return _mapper.Map<TaskTimeoutRuleDto>(taskTimeoutRule);
    }

    /// <summary>
    /// 查询任务超时规则
    /// </summary>
    /// <param name="taskType">任务类型</param>
    /// <returns></returns>
    public async Task<TaskTimeoutRuleDto> GetTaskTimeoutRuleByTaskTypeAsync(string taskType)
    {
        var taskTimeoutRule = await _taskTimeoutRuleRepository.GetByTaskTypeAsync(taskType);
        return _mapper.Map<TaskTimeoutRuleDto>(taskTimeoutRule);
    }
    
    /// <summary>
    /// 查询任务超时规则
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<TaskTimeoutRuleDto>> GetAllTaskTimeoutRulesAsync()
    {
        var taskTimeoutRules = _taskTimeoutRuleRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<TaskTimeoutRuleDto>>(taskTimeoutRules);
    }
}