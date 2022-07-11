using Microsoft.AspNetCore.Mvc;
using Serilog;
using WeatherInformation.Application.Contracts;
using WeatherInformation.Domain.Dto.Request;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace WeatherInformation.API.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MeasurementDataController : ControllerBase
    {
        private readonly IMeasurementDataService _measurementDataService;

        public MeasurementDataController(IMeasurementDataService measurementDataService)
        {
            _measurementDataService = measurementDataService;
        }

        [HttpGet("getdata")]
        [ProducesResponseType(typeof(string), Status200OK)]
        [ProducesResponseType(typeof(string), Status404NotFound)]
        [ProducesResponseType(typeof(string), Status400BadRequest)]
        public async Task<IActionResult> GetDataAsync([FromQuery] GetDataRequestDto request)
        {
            try
            {
                Log.Information($"Starting method {nameof(GetDataAsync)}");

                var result = await _measurementDataService.GetDataByDeviceSensorTypeAndDay(request);

                return Ok(result);
            }
            catch (Exception exc)
            {
                Log.Error($"An error ocurred while processing method {nameof(GetDataAsync)}", exc);
                return BadRequest($"{exc.Message}");
            }
            finally
            {
                Log.Information($"Method {nameof(GetDataAsync)} executed.");
            }
        }

        [HttpGet("getdatafordevice")]
        [ProducesResponseType(typeof(IEnumerable<string>), Status200OK)]
        [ProducesResponseType(typeof(string), Status404NotFound)]
        [ProducesResponseType(typeof(string), Status400BadRequest)]
        public async Task<IActionResult> GetDataForDeviceAsync([FromQuery] GetDataForDeviceRequestDto request)
        {
            try
            {
                Log.Information($"Starting method {nameof(GetDataForDeviceAsync)}");

                var result = await _measurementDataService.GetDataByDeviceAndDay(request);

                return Ok(result);
            }
            catch (Exception exc)
            {
                Log.Error($"An error ocurred while processing method {nameof(GetDataAsync)}", exc);
                return BadRequest($"{exc.Message}");
            }
            finally
            {
                Log.Information($"Method {nameof(GetDataForDeviceAsync)} executed.");
            }
        }
    }
}
