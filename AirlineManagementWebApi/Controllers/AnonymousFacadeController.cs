﻿using AirlineManagement;
using AirlineManagementWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace AirlineManagementWebApi.Controllers
{
    public class AnonymousFacadeController : ApiController
    {
        private FlyingCenterSystem FCS;
        /// <summary>
        /// Get all flights
        /// </summary>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(Flight))]
        [Route("api/AnonymousFacade/allflights")]
        [HttpGet]
        public IHttpActionResult GetAllFlights()
        {
            FCS = FlyingCenterSystem.GetFlyingCenterSystemInstance();
            IAnonymousUserFacade anonymousFacade = FCS.GetFacade(null) as IAnonymousUserFacade;
            IList<Flight> flights = anonymousFacade.GetAllFlights();
            if (flights.Count == 0)
            {
                return NotFound();
            }
            return Ok(flights);
        }
        /// <summary>
        /// Get all airline companies
        /// </summary>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(AirlineCompany))]
        [Route("api/AnonymousFacade/allairlinecompanies")]
        [HttpGet]
        public IHttpActionResult GetAllAirlineCompanies()
        {
            FCS = FlyingCenterSystem.GetFlyingCenterSystemInstance();
            IAnonymousUserFacade anonymousFacade = FCS.GetFacade(null) as IAnonymousUserFacade;
            IList<AirlineCompany> airlineCompanies = anonymousFacade.GetAllAirlineCompanies();
            if (airlineCompanies.Count == 0)
            {
                return NotFound();
            }
            return Ok(airlineCompanies);
        }
        /// <summary>
        ///  Get all airline companies by scheduled Time
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        [ResponseType(typeof(AirlineCompany))]
        [Route("api/AnonymousFacade/allairlinecompanies/search")]
        [HttpGet]
        public IHttpActionResult GetAllAirlineCompaniesByScheduledTime(string typeName)
        {
            FCS = FlyingCenterSystem.GetFlyingCenterSystemInstance();
            IAnonymousUserFacade anonymousFacade = FCS.GetFacade(null) as IAnonymousUserFacade;
            IList<AirlineCompany> airlineCompanies = anonymousFacade.GetAllAirlineCompaniesByScheduledTime(typeName);
            if (airlineCompanies.Count == 0)
            {
                return NotFound();
            }
            return Ok(airlineCompanies);
        }
        /// <summary>
        /// Get flights by vacancy
        /// </summary>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(Flight))]
        [Route("api/AnonymousFacade/gebyvacancy/{id}")]
        [HttpGet]
        public IHttpActionResult GetAllFlightsVacancy()
        {
            FCS = FlyingCenterSystem.GetFlyingCenterSystemInstance();
            IAnonymousUserFacade anonymousFacade = FCS.GetFacade(null) as IAnonymousUserFacade;
            Dictionary<Flight, int> flights = anonymousFacade.GetAllFlightsByVacancy();

            if (flights.Count == 0)
            {
                return NotFound();
            }
            return Ok(flights);
        }
        /// <summary>
        /// Get flight by id
        /// </summary>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(Flight))]
        [Route("api/AnonymousFacade/getbyid/{flightId}")]
        [HttpGet]
        public IHttpActionResult GetFlightById(int flightId)
        {
            FCS = FlyingCenterSystem.GetFlyingCenterSystemInstance();
            IAnonymousUserFacade anonymousFacade = FCS.GetFacade(null) as IAnonymousUserFacade;
            Flight flight = anonymousFacade.GetFlightById(flightId);

            if (flight == null)
            {
                return NotFound();
            }
            return Ok(flight);
        }
        /// <summary>
        /// Search with parameters (GET)
        /// </summary>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(Flight))]
        [Route("api/AnonymousFacade/SearchFlightByConditions/search")]
        [HttpGet]
        public IHttpActionResult SearchFlightByConditions(string typeName, string flightId, string country, string company)
        {
            
            IList<Flight> flights = null;
            FCS = FlyingCenterSystem.GetFlyingCenterSystemInstance();
            IAnonymousUserFacade anonymousFacade = FCS.GetFacade(null) as IAnonymousUserFacade;
         
            flights = anonymousFacade.GetAllFilteredFlights(typeName, flightId, country, company);
            if (flights == null)
            {
                return NotFound();
            }
            SetRandomDepartureDelayedStatus(flights);
            SetRandomArrivalDelayedStatus(flights);
            return Ok(flights);
        }

        /// <summary>
        /// Get flights by origin country code
        /// </summary>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(Flight))]
        [Route("api/AnonymousFacade/getbyorigincountrycode/{countryCode}")]
        [HttpGet]
        public IHttpActionResult GetFlightsByOriginCountry(int countryCode)
        {
            FCS = FlyingCenterSystem.GetFlyingCenterSystemInstance();
            IAnonymousUserFacade anonymousFacade = FCS.GetFacade(null) as IAnonymousUserFacade;
            IList<Flight> flights = anonymousFacade.GetFlightsByOriginCountry(countryCode);

            if (flights.Count == 0)
            {
                return NotFound();
            }
            return Ok(flights);
        }
        /// <summary>
        /// Get flights by destination country code (Query parameters)
        /// </summary>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(Flight))]
        [Route("api/AnonymousFacade/getbydestinationcountrycode/search")]
        [HttpGet]
        public IHttpActionResult GetFlightsByDestinationCountry(int countryCode = 0)
        {
            IHttpActionResult res = null;
            IList<Flight> flights = null;
            FCS = FlyingCenterSystem.GetFlyingCenterSystemInstance();
            IAnonymousUserFacade anonymousFacade = FCS.GetFacade(null) as IAnonymousUserFacade;
            if (countryCode != 0)
            {
                flights = anonymousFacade.GetFlightsByDestinationCountry(countryCode);
                res = Ok(flights);
            }
            else if ((countryCode != 0 && flights.Count == 0) || countryCode == 0)
            {
                res = NotFound();
            }

            return res;
        }
        /// <summary>
        /// Get flights by departure date
        /// </summary>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(Flight))]
        [Route("api/AnonymousFacade/getbydeparturedate/{departureDate}")]
        [HttpGet]
        public IHttpActionResult GetFlightsByDepatrureDate(DateTime departureDate)
        {
            FCS = FlyingCenterSystem.GetFlyingCenterSystemInstance();
            IAnonymousUserFacade anonymousFacade = FCS.GetFacade(null) as IAnonymousUserFacade;
            IList<Flight> flights = anonymousFacade.GetFlightsByDepatrureDate(departureDate);

            if (flights.Count == 0)
            {
                return NotFound();
            }
            return Ok(flights);
        }
        /// <summary>
        /// Get flights by landinge date
        /// </summary>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(Flight))]
        [Route("api/AnonymousFacade/getbylandingdate/{landingeDate}")]
        [HttpGet]
        public IHttpActionResult GetFlightsByLandingDate(DateTime landingeDate)
        {
            FCS = FlyingCenterSystem.GetFlyingCenterSystemInstance();
            IAnonymousUserFacade anonymousFacade = FCS.GetFacade(null) as IAnonymousUserFacade;
            IList<Flight> flights = anonymousFacade.GetFlightsByLandingDate(landingeDate);

            if (flights.Count == 0)
            {
                return NotFound();
            }
            return Ok(flights);
        }
        /// <summary>
        /// Get random order list
        /// </summary>
        /// <param name="from"></param>
        /// <param name="rangeEx"></param>
        /// <returns></returns>
        private int[] RandomNumber(int from, int rangeEx)
        {
            var orderedList = Enumerable.Range(from, rangeEx);
            var rng = new Random();
            return orderedList.OrderBy(c => rng.Next()).ToArray();
        }
        /// <summary>
        /// Set random departure delayes status
        /// </summary>
        /// <param name="flights"></param>
        public void SetRandomDepartureDelayedStatus(IList<Flight> flights)
        {
            Random random = new Random();
            int perpercentage;
            int randomMinutes;
            int numOfFlights;
            FCS = FlyingCenterSystem.GetFlyingCenterSystemInstance();
            IAnonymousUserFacade anonymousFacade = FCS.GetFacade(null) as IAnonymousUserFacade;
            Flight[] flightsArray = new Flight[flights.Count];
            int[] flightsRandomIndex = new int[flights.Count];
            perpercentage = random.Next(10, 21);

            int m = 0;
            foreach (Flight f in flights)
            {
                if (f.REAL_DEPARTURE_TIME == f.DEPARTURE_TIME)
                {
                    flightsArray[m] = f;
                    m++;
                }
            }
            if (flights.Count >= 5)
            {
                numOfFlights = (flightsArray.Length * perpercentage) / 100;

                if (numOfFlights == 0)
                {
                    numOfFlights = 1;
                }
                numOfFlights = numOfFlights - flights.Count + m;
                if (numOfFlights < 0)
                {
                    numOfFlights = 0;
                }
                flightsRandomIndex = RandomNumber(0, flightsArray.Length - 1);
            }
            else
            {
                numOfFlights = 0;
            }
            if (numOfFlights > 0)
            {
                for (int i = 0; i < numOfFlights; i++)
                {
                    int index = flightsRandomIndex[i];
                    
                    randomMinutes = random.Next(30, 241);
                    flightsArray[index].REAL_DEPARTURE_TIME = flightsArray[index].DEPARTURE_TIME.AddMinutes(randomMinutes);
                    flightsArray[index].DEPARTURE_TIME_DIFF = flightsArray[index].DEPARTURE_TIME.Subtract(flightsArray[index].REAL_DEPARTURE_TIME);
                    anonymousFacade.UpdateRealDepartureTime(flightsArray[index].ID, flightsArray[index].REAL_DEPARTURE_TIME);
                }
            }
        }
        /// <summary>
        /// Set random arrivals delayes status
        /// </summary>
        /// <param name="arrivalFlights"></param>
        public void SetRandomArrivalDelayedStatus(IList<Flight> arrivalFlights)
        {
            Random random = new Random();
            int perpercentage;
            int randomMinutes;
            int numOfFlights;
            FCS = FlyingCenterSystem.GetFlyingCenterSystemInstance();
            IAnonymousUserFacade anonymousFacade = FCS.GetFacade(null) as IAnonymousUserFacade;
            Flight[] flightsArray = new Flight[arrivalFlights.Count];
            int[] flightsRandomIndex = new int[arrivalFlights.Count];
            DateTime localDate = DateTime.Now;
            perpercentage = random.Next(10, 21);
    
            int m = 0;
            int nf = 0;
            foreach (Flight f in arrivalFlights)
            {
                if (localDate < f.REAL_LANDING_TIME.AddHours(-2))
                {
                    nf++;
                    if (f.REAL_LANDING_TIME == f.LANDING_TIME)
                    {


                        flightsArray[m] = f;
                        m++;

                    }
                } 
            }
            if(m > 1)
            {
                if (flightsArray.Length >= 1)
                {

                    numOfFlights = (nf * perpercentage) / 100;

                    if (numOfFlights == 0)
                    {
                        numOfFlights = 1;
                    }
                    numOfFlights = numOfFlights - (nf - m);

                    if (numOfFlights < 0)
                    {
                        numOfFlights = 0;
                    }
                    flightsRandomIndex = RandomNumber(0, m - 1);
                }
                else
                {
                    numOfFlights = 0;
                }
                if (numOfFlights > 0)
                {
                    for (int i = 0; i < numOfFlights; i++)
                    {
                        int index = flightsRandomIndex[i];

                        randomMinutes = random.Next(30, 241);
                        flightsArray[index].REAL_LANDING_TIME = flightsArray[index].LANDING_TIME.AddMinutes(randomMinutes);
                        flightsArray[index].LANDING_TIME_DIFF = flightsArray[index].LANDING_TIME.Subtract(flightsArray[index].REAL_LANDING_TIME);
                        anonymousFacade.UpdateRealArrivalTime(flightsArray[index].ID, flightsArray[index].REAL_LANDING_TIME);
                    }
                }
            }
            
        }
        /// <summary>
        /// Get all coutries
        /// </summary>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(Country))]
        [Route("api/AnonymousFacade/allCountries")]
        [HttpGet]
        public IHttpActionResult GetAllCountries()
        {
            FCS = FlyingCenterSystem.GetFlyingCenterSystemInstance();
            IAnonymousUserFacade anonymousFacade = FCS.GetFacade(null) as IAnonymousUserFacade;
            IList<Country> countries = anonymousFacade.GetAllCountries();
            if (countries.Count == 0)
            {
                return NotFound();
            }
            return Ok(countries);
        }
        /// <summary>
        /// Get all coutries by scheduled time
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(Country))]
        [Route("api/AnonymousFacade/allCountriesByScheduledTime/search")]
        [HttpGet]
        public IHttpActionResult GetAllCountriesByScheduledTime(string typeName)
        {
            FCS = FlyingCenterSystem.GetFlyingCenterSystemInstance();
            IAnonymousUserFacade anonymousFacade = FCS.GetFacade(null) as IAnonymousUserFacade;
            IList<Country> countries = anonymousFacade.GetAllCountriesByScheduledTime(typeName);
            if (countries.Count == 0)
            {
                return NotFound();
            }
            return Ok(countries);
        }
        /// <summary>
        /// Get all flight Ids
        /// </summary>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(long))]
        [Route("api/AnonymousFacade/allFlightIds")]
        [HttpGet]
        public IHttpActionResult GetAllFlightIds()
        {
            FCS = FlyingCenterSystem.GetFlyingCenterSystemInstance();
            IAnonymousUserFacade anonymousFacade = FCS.GetFacade(null) as IAnonymousUserFacade;
            IList<long> fligthIds = anonymousFacade.GetAllFlightsIds();
            if (fligthIds.Count == 0)
            {
                return NotFound();
            }
            return Ok(fligthIds);
        }
        /// <summary>
        /// Get all flight Ids by scheduled time
        /// </summary>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(long))]
        [Route("api/AnonymousFacade/allFlightIdsByScheduledTime/search")]
        [HttpGet]
        public IHttpActionResult GetFlightIdsByScheduledTime(string typeName)
        {
            FCS = FlyingCenterSystem.GetFlyingCenterSystemInstance();
            IAnonymousUserFacade anonymousFacade = FCS.GetFacade(null) as IAnonymousUserFacade;
            IList<long> fligthIds = anonymousFacade.GetFlightIdsByScheduledTime(typeName);
            if (fligthIds.Count == 0)
            {
                return NotFound();
            }
            return Ok(fligthIds);
        }
        /// <summary>
        /// Search with parameters (POST)
        /// </summary>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(Flight))]
        [Route("api/AnonymousFacade/searchbydata", Name = "searchbydata")]
        [HttpPost]
        public IHttpActionResult SearchWithParametars([FromBody] SearchParameters searchData)
        {
            IList<Flight> flights = null;
            FCS = FlyingCenterSystem.GetFlyingCenterSystemInstance();
            IAnonymousUserFacade anonymousFacade = FCS.GetFacade(null) as IAnonymousUserFacade;
            SearchParameters searchParameters = new SearchParameters();
            searchParameters.airlineCompany = searchData.airlineCompany;
            searchParameters.originCountry = searchData.originCountry;
            searchParameters.flightId = searchData.flightId;
            searchParameters.arrivalsDepartures = searchData.arrivalsDepartures;
            //string typeName, string flightId, string country, string company
            flights = anonymousFacade.GetAllFilteredFlights(searchParameters.arrivalsDepartures, searchParameters.flightId, searchParameters.originCountry, searchParameters.airlineCompany);
        
            if (flights == null)
            {
                return NotFound();
            }
            SetRandomDepartureDelayedStatus(flights);
            SetRandomArrivalDelayedStatus(flights);
            return Ok(flights);
        }
    }

}
