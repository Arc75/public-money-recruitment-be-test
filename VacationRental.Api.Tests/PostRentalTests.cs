using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Services.Models;
using Xunit;

namespace VacationRental.Api.Tests
{
    [Collection("Integration")]
    public class PostRentalTests
    {
        private readonly HttpClient _client;

        public PostRentalTests(IntegrationFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostRental_ThenAGetReturnsTheCreatedRental()
        {
            var request = new RentalBindingModel
            {
                Units = 25,
                PreparationTimeInDays = 2
            };

            ResourceIdViewModel postResult;
            using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", request))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
                postResult = await postResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            using (var getResponse = await _client.GetAsync($"/api/v1/rentals/{postResult.Id}"))
            {
                Assert.True(getResponse.IsSuccessStatusCode);

                var getResult = await getResponse.Content.ReadAsAsync<RentalViewModel>();
                Assert.Equal(request.Units, getResult.Units);
            }
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPutRental_ThenAPostReturnsSuccess()
        {
            var request = new RentalBindingModel
            {
                Units = 1,
                PreparationTimeInDays = 1
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", request))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBooking1Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 3,
                Start = new DateTime(2022, 01, 01)
            };

            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
            }

            var postBooking2Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 1,
                Start = new DateTime(2022, 01, 04)
            };

            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
            }

            var putRequest = new RentalBindingModel
            {
                Id = postRentalResult.Id,
                Units = 2,
                PreparationTimeInDays = 1
            };

            using (var postRentalResponse = await _client.PutAsJsonAsync($"/api/v1/rentals", putRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
            }
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPutRental_ThenAPostReturnsErrorOverbooking()
        {
            var request = new RentalBindingModel
            {
                Units = 1,
                PreparationTimeInDays = 1
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", request))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBooking1Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 3,
                Start = new DateTime(2022, 01, 01)
            };

            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
            }

            var postBooking2Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 1,
                Start = new DateTime(2022, 01, 04)
            };

            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
            }

            var putRequest = new RentalBindingModel
            {
                Id = postRentalResult.Id,
                Units = 1,
                PreparationTimeInDays = 3
            };

            await Assert.ThrowsAsync<ApplicationException>(async () =>
            {
                using (var postRentalResponse = await _client.PutAsJsonAsync($"/api/v1/rentals", putRequest))
                {
                }
            });
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPutRental_ThenAPostReturnsErrorCantReduceUnits()
        {
            var request = new RentalBindingModel
            {
                Units = 2,
                PreparationTimeInDays = 1
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", request))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBooking1Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 3,
                Start = new DateTime(2022, 01, 01)
            };

            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
            }

            var postBooking2Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 2,
                Start = new DateTime(2022, 01, 01)
            };

            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
            }

            var putRequest = new RentalBindingModel
            {
                Id = postRentalResult.Id,
                Units = 1,
                PreparationTimeInDays = 3
            };

            await Assert.ThrowsAsync<ApplicationException>(async () =>
            {
                using (var postRentalResponse = await _client.PutAsJsonAsync($"/api/v1/rentals", putRequest))
                {
                }
            });
        }
    }
}
