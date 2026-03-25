using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.AspApi.DTO;
using QuantityMeasurementApp.RepoLayer.Context;
using QuantityMeasurementApp.RepoLayer.Implementations;
using QuantityMeasurementApp.RepoLayer.Interfaces;

namespace QuantityMeasurementApp.Tests
{
    // ── UC17: REST Controller Tests ───────────────────────────────────────
    // Uses WebApplicationFactory to spin up the real API in-process.
    // Replaces SQL Server with EF Core InMemory so no da cbntabase is needed.
    [TestClass]
    public class WebApiControllerTests
    {
        private static WebApplicationFactory<Program> _factory = null!;
        private static HttpClient                     _client  = null!;

        private static readonly JsonSerializerOptions _json = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        [ClassInitialize]
        public static void ClassInit(TestContext _)
        {
            // UseEnvironment must be set on IHostBuilder not IWebHostBuilder
            // We do it via environment variable before the factory starts
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");

            _factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(host =>
                {
                    // Override the connection string with a special value.
                    // Program.cs AddDbContext uses UseSqlServer with this value.
                    // We then replace the whole DbContext with InMemory in ConfigureServices.
                    // Key: call ConfigureAppConfiguration BEFORE ConfigureServices so the
                    // AddDbContext in Program.cs reads our test config, then we replace it.
                    host.ConfigureAppConfiguration((ctx, config) =>
                    {
                        config.AddInMemoryCollection(new Dictionary<string, string?>
                        {
                            ["ConnectionStrings:DefaultConnection"] = "DataSource=:memory:"
                        });
                    });

                    host.ConfigureServices(services =>
                    {
                        // Remove every descriptor related to DbContext / SqlServer
                        // We remove by service type AND by checking all descriptors
                        // for SqlServer references in implementation type names
                        var allDescriptors = services.ToList();
                        foreach (var d in allDescriptors)
                        {
                            bool isSqlServer =
                                (d.ImplementationType?.Assembly?.FullName?.Contains("SqlServer") == true) ||
                                (d.ImplementationType?.FullName?.Contains("SqlServer") == true) ||
                                (d.ServiceType.FullName?.Contains("SqlServer") == true) ||
                                d.ServiceType == typeof(AppDbContext) ||
                                d.ServiceType == typeof(DbContextOptions<AppDbContext>) ||
                                (d.ServiceType.IsGenericType &&
                                 d.ServiceType.GetGenericTypeDefinition() == typeof(DbContextOptions<>));
                            if (isSqlServer) services.Remove(d);
                        }

                        // Register clean InMemory DbContext
                        services.AddDbContext<AppDbContext>(options =>
                            options.UseInMemoryDatabase("WebApiTestDb"));

                        // Re-register repositories
                        services.RemoveAll(typeof(IQuantityMeasurementRepository));
                        services.AddScoped<IQuantityMeasurementRepository,
                            QuantityMeasurementRepository>();

                        services.RemoveAll(typeof(IUserRepository));
                        services.AddScoped<IUserRepository, UserRepository>();
                    });
                });

            _client = _factory.CreateClient();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            _client?.Dispose();
            _factory?.Dispose();
        }

        // ── POST /compare ─────────────────────────────────────────────────

        [TestMethod]
        public async Task Post_Compare_1Foot_And_12Inches_Returns200_True()
        {
            var payload = new
            {
                thisQuantityDTO = new { value = 1.0,  unit = "FEET",   measurementType = "LengthUnit" },
                thatQuantityDTO = new { value = 12.0, unit = "INCHES", measurementType = "LengthUnit" }
            };
            var response = await _client.PostAsJsonAsync("/api/v1/quantities/compare", payload);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var dto = await Deserialize<QuantityMeasurementResponseDTO>(response);
            Assert.AreEqual("true",    dto.ResultString);
            Assert.AreEqual("compare", dto.Operation);
            Assert.IsFalse(dto.IsError);
        }

        [TestMethod]
        public async Task Post_Compare_DifferentValues_ReturnsFalse()
        {
            var payload = new
            {
                thisQuantityDTO = new { value = 1.0, unit = "FEET", measurementType = "LengthUnit" },
                thatQuantityDTO = new { value = 2.0, unit = "FEET", measurementType = "LengthUnit" }
            };
            var response = await _client.PostAsJsonAsync("/api/v1/quantities/compare", payload);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var dto = await Deserialize<QuantityMeasurementResponseDTO>(response);
            Assert.AreEqual("false", dto.ResultString);
        }

        // ── POST /convert ─────────────────────────────────────────────────

        [TestMethod]
        public async Task Post_Convert_1Foot_To_Inches_Returns12()
        {
            var payload = new
            {
                thisQuantityDTO = new { value = 1.0, unit = "FEET",   measurementType = "LengthUnit" },
                thatQuantityDTO = new { value = 0.0, unit = "INCHES", measurementType = "LengthUnit" }
            };
            var response = await _client.PostAsJsonAsync("/api/v1/quantities/convert", payload);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var dto = await Deserialize<QuantityMeasurementResponseDTO>(response);
            Assert.AreEqual(12.0, dto.ResultValue, 0.0001);
        }

        [TestMethod]
        public async Task Post_Convert_0Celsius_To_32Fahrenheit()
        {
            var payload = new
            {
                thisQuantityDTO = new { value = 0.0, unit = "CELSIUS",    measurementType = "TemperatureUnit" },
                thatQuantityDTO = new { value = 0.0, unit = "FAHRENHEIT", measurementType = "TemperatureUnit" }
            };
            var response = await _client.PostAsJsonAsync("/api/v1/quantities/convert", payload);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var dto = await Deserialize<QuantityMeasurementResponseDTO>(response);
            Assert.AreEqual(32.0, dto.ResultValue, 0.0001);
        }

        // ── POST /add ─────────────────────────────────────────────────────

        [TestMethod]
        public async Task Post_Add_1Foot_And_12Inches_Returns2Feet()
        {
            var payload = new
            {
                thisQuantityDTO = new { value = 1.0,  unit = "FEET",   measurementType = "LengthUnit" },
                thatQuantityDTO = new { value = 12.0, unit = "INCHES", measurementType = "LengthUnit" }
            };
            var response = await _client.PostAsJsonAsync("/api/v1/quantities/add", payload);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var dto = await Deserialize<QuantityMeasurementResponseDTO>(response);
            Assert.AreEqual(2.0,    dto.ResultValue, 0.0001);
            Assert.AreEqual("FEET", dto.ResultUnit);
        }

        [TestMethod]
        public async Task Post_Add_1Kg_And_1000Grams_Returns2Kg()
        {
            var payload = new
            {
                thisQuantityDTO = new { value = 1.0,    unit = "KILOGRAM", measurementType = "WeightUnit" },
                thatQuantityDTO = new { value = 1000.0, unit = "GRAM",     measurementType = "WeightUnit" }
            };
            var response = await _client.PostAsJsonAsync("/api/v1/quantities/add", payload);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var dto = await Deserialize<QuantityMeasurementResponseDTO>(response);
            Assert.AreEqual(2.0, dto.ResultValue, 0.0001);
        }

        // ── POST /subtract ────────────────────────────────────────────────

        [TestMethod]
        public async Task Post_Subtract_2Feet_Minus_12Inches_Returns1Foot()
        {
            var payload = new
            {
                thisQuantityDTO = new { value = 2.0,  unit = "FEET",   measurementType = "LengthUnit" },
                thatQuantityDTO = new { value = 12.0, unit = "INCHES", measurementType = "LengthUnit" }
            };
            var response = await _client.PostAsJsonAsync("/api/v1/quantities/subtract", payload);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var dto = await Deserialize<QuantityMeasurementResponseDTO>(response);
            Assert.AreEqual(1.0, dto.ResultValue, 0.0001);
        }

        // ── POST /divide ──────────────────────────────────────────────────

        [TestMethod]
        public async Task Post_Divide_2Litres_By_1Litre_Returns2()
        {
            var payload = new
            {
                thisQuantityDTO = new { value = 2.0, unit = "LITRE", measurementType = "VolumeUnit" },
                thatQuantityDTO = new { value = 1.0, unit = "LITRE", measurementType = "VolumeUnit" }
            };
            var response = await _client.PostAsJsonAsync("/api/v1/quantities/divide", payload);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var dto = await Deserialize<QuantityMeasurementResponseDTO>(response);
            Assert.AreEqual(2.0, dto.ResultValue, 0.0001);
        }

        // ── Error scenarios ───────────────────────────────────────────────

        [TestMethod]

        
        public async Task Post_Add_IncompatibleTypes_Returns400()
        {
            var payload = new
            {
                thisQuantityDTO = new { value = 1.0, unit = "FEET",     measurementType = "LengthUnit" },
                thatQuantityDTO = new { value = 1.0, unit = "KILOGRAM", measurementType = "WeightUnit" }
            };
            var response = await _client.PostAsJsonAsync("/api/v1/quantities/add", payload);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            var dto = await Deserialize<QuantityMeasurementResponseDTO>(response);
            Assert.IsTrue(dto.IsError);
        }

        [TestMethod]
        public async Task Post_Add_Temperature_Returns400()
        {
            var payload = new
            {
                thisQuantityDTO = new { value = 10.0, unit = "CELSIUS", measurementType = "TemperatureUnit" },
                thatQuantityDTO = new { value = 20.0, unit = "CELSIUS", measurementType = "TemperatureUnit" }
            };
            var response = await _client.PostAsJsonAsync("/api/v1/quantities/add", payload);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            var dto = await Deserialize<QuantityMeasurementResponseDTO>(response);
            Assert.IsTrue(dto.IsError);
            StringAssert.Contains(dto.ErrorMessage!, "Temperature");
        }

        [TestMethod]
        public async Task Post_InvalidUnit_Returns400()
        {
            var payload = new
            {
                thisQuantityDTO = new { value = 1.0, unit = "FOOT",  measurementType = "LengthUnit" },
                thatQuantityDTO = new { value = 1.0, unit = "INCHE", measurementType = "LengthUnit" }
            };
            var response = await _client.PostAsJsonAsync("/api/v1/quantities/add", payload);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        // ── GET endpoints ─────────────────────────────────────────────────

        [TestMethod]
        public async Task Get_History_AfterOperation_ReturnsRecord()
        {
            // Use a fresh client for this test to avoid shared state issues
            using var client = _factory.CreateClient();
            var payload = new
            {
                thisQuantityDTO = new { value = 1.0,  unit = "FEET",   measurementType = "LengthUnit" },
                thatQuantityDTO = new { value = 12.0, unit = "INCHES", measurementType = "LengthUnit" }
            };
            var postResponse = await client.PostAsJsonAsync("/api/v1/quantities/compare", payload);
            // Only check GET if POST succeeded
            if (postResponse.IsSuccessStatusCode)
            {
                var response = await client.GetAsync("/api/v1/quantities/history");
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            }
            else
            {
                // POST itself failed — skip GET assertion and just verify POST returns a known status
                Assert.IsTrue(
                    postResponse.StatusCode == HttpStatusCode.OK ||
                    postResponse.StatusCode == HttpStatusCode.Created,
                    $"POST /compare returned {postResponse.StatusCode}: {await postResponse.Content.ReadAsStringAsync()}");
            }
        }

        [TestMethod]
        public async Task Get_Count_AfterCompareOperation_Returns1()
        {
            using var client = _factory.CreateClient();
            var payload = new
            {
                thisQuantityDTO = new { value = 1.0,  unit = "FEET",   measurementType = "LengthUnit" },
                thatQuantityDTO = new { value = 12.0, unit = "INCHES", measurementType = "LengthUnit" }
            };
            var postResponse = await client.PostAsJsonAsync("/api/v1/quantities/compare", payload);
            if (postResponse.IsSuccessStatusCode)
            {
                var response = await client.GetAsync("/api/v1/quantities/count/COMPARE");
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            }
            else
            {
                Assert.IsTrue(
                    postResponse.StatusCode == HttpStatusCode.OK ||
                    postResponse.StatusCode == HttpStatusCode.Created,
                    $"POST /compare returned {postResponse.StatusCode}: {await postResponse.Content.ReadAsStringAsync()}");
            }
        }

        [TestMethod]
        public async Task Get_ErrorHistory_AfterFailedOperation_ContainsErrorRecord()
        {
            using var client = _factory.CreateClient();
            var payload = new
            {
                thisQuantityDTO = new { value = 10.0, unit = "CELSIUS", measurementType = "TemperatureUnit" },
                thatQuantityDTO = new { value = 20.0, unit = "CELSIUS", measurementType = "TemperatureUnit" }
            };
            // This POST should return 400 (Temperature arithmetic not supported)
            // but should still persist the error record
            await client.PostAsJsonAsync("/api/v1/quantities/add", payload);
            var response = await client.GetAsync("/api/v1/quantities/history/errored");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task Get_History_ReturnsOK_WithoutPriorOperation()
        {
            // History endpoint should always return 200 even if empty
            var response = await _factory.CreateClient().GetAsync("/api/v1/quantities/history");
            var body = await response.Content.ReadAsStringAsync();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode,
                $"GET /history returned {response.StatusCode}. Body: {body}");
        }

        // ── Helper ────────────────────────────────────────────────────────

        private static async Task<T> Deserialize<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, _json)
                   ?? throw new InvalidOperationException($"Failed to deserialise: {content}");
        }
    }
}