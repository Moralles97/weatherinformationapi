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

        [HttpGet("getData")]
        [ProducesResponseType(typeof(File), Status200OK)]
        [ProducesResponseType(typeof(EmptyResult), Status200OK)]
        [ProducesResponseType(typeof(string), Status404NotFound)]
        [ProducesResponseType(typeof(string), Status400BadRequest)]
        public async Task<IActionResult> GetDataAsync([FromQuery] GetDataRequestDto request)
        {
            try
            {
                Log.Information($"Starting method {nameof(GetDataAsync)}");

                var result = await _measurementDataService.GetDataByDeviceSensorTypeAndDayAsync(request);

                if (result is null)
                    return new EmptyResult();

                return File(result, "application/octet-stream", $"{request.DeviceId}-{request.SensorType}-{request.Date:yyyy-MM-dd}.csv");
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

        [HttpGet("getCompressedData")]
        [ProducesResponseType(typeof(File), Status200OK)]
        [ProducesResponseType(typeof(string), Status404NotFound)]
        [ProducesResponseType(typeof(string), Status400BadRequest)]
        public async Task<IActionResult> GetCompressedDataAsync([FromQuery] GetCompressedDataRequestDto request)
        {
            try
            {
                Log.Information($"Starting method {nameof(GetCompressedDataAsync)}");

                var result = await _measurementDataService.GetCompressedDataByDeviceAndSensorTypeAsync(request);

                if (result is null)
                    return new EmptyResult();

                return File(result, "application/octet-stream", $"{request.DeviceId}-{request.SensorType}.zip");
            }
            catch (Exception exc)
            {
                Log.Error($"An error ocurred while processing method {nameof(GetCompressedDataAsync)}", exc);
                return BadRequest($"{exc.Message}");
            }
            finally
            {
                Log.Information($"Method {nameof(GetCompressedDataAsync)} executed.");
            }
        }

        [HttpGet("getDataForDevice")]
        [ProducesResponseType(typeof(IEnumerable<string>), Status200OK)]
        [ProducesResponseType(typeof(string), Status404NotFound)]
        [ProducesResponseType(typeof(string), Status400BadRequest)]
        public async Task<IActionResult> GetDataForDeviceAsync([FromQuery] GetDataForDeviceRequestDto request)
        {
            try
            {
                Log.Information($"Starting method {nameof(GetDataForDeviceAsync)}");

                var result = await _measurementDataService.GetDataByDeviceAndDayAsync(request);

                return File(result, "application/octet-stream", $"{request.DeviceId}-{request.Date:yyyy-MM-dd}.zip");
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
