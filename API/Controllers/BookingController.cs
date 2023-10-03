using API.Contracts;
using API.DTOs.Bookings;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingController(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _bookingRepository.GetAll();

            if (!result.Any())
            {
                return NotFound("Data Not Found");
            }

            var data = result.Select(x => (BookingDto)x);

            return Ok(data);
        }

        [HttpGet("{guid}")]
        public IActionResult GetByGuid(Guid guid)
        {
            var result = _bookingRepository.GetByGuid(guid);

            if (result is null)
            {
                return NotFound("Id Not Found");
            }

            return Ok((BookingDto)result);
        }

        [HttpPost]
        public IActionResult Create(CreateBookingDto bookingDto)
        {
            var result = _bookingRepository.Create(bookingDto);

            if (result is null)
            {
                return BadRequest("Failed to create data");
            }

            return Ok((BookingDto)result);
        }

        [HttpPut]
        public IActionResult Update(BookingDto bookingDto)
        {
            var entity = _bookingRepository.GetByGuid(bookingDto.Guid);

            if (entity is null)
            {
                return NotFound("Id Not Found");
            }

            Booking toUpdate = bookingDto;
            toUpdate.CreatedDate = entity.CreatedDate;

            var result = _bookingRepository.Update(toUpdate);

            if (!result)
            {
                return BadRequest("Failed to update data");
            }

            return Ok("Data Updated");
        }

        [HttpDelete("{guid}")]
        public IActionResult Delete(Guid guid)
        {
            var entity = _bookingRepository.GetByGuid(guid);

            if (entity is null)
            {
                return NotFound("Id Not Found");
            }

            var result = _bookingRepository.Delete(entity);

            if (!result)
            {
                return BadRequest("Failed to delete data");
            }

            return Ok("Data Deleted");
        }
    }
}