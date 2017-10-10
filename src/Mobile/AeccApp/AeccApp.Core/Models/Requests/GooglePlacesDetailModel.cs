using System;
using System.Collections.Generic;
using System.Text;

namespace AeccApp.Core.Models.Requests
{
    using System;
    using System.Net;
    using System.Collections.Generic;

    using Newtonsoft.Json;
    /// <summary>
    /// GooglePlacesDetailModel represents the deserialized JSON returned by GooglePlaces API
    /// This one does contains coordinates, used manage map pins.
    /// </summary>
    public class PlacesDetailModel
    {
        [JsonProperty("result")]
        public Result Result { get; set; }

        [JsonProperty("html_attributions")]
        public object[] HtmlAttributions { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public class Result
    {
        [JsonProperty("international_phone_number")]
        public string InternationalPhoneNumber { get; set; }

        [JsonProperty("formatted_phone_number")]
        public string FormattedPhoneNumber { get; set; }

        [JsonProperty("alt_ids")]
        public AltId[] AltIds { get; set; }

        [JsonProperty("address_components")]
        public AddressComponent[] AddressComponents { get; set; }

        [JsonProperty("formatted_address")]
        public string FormattedAddress { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("geometry")]
        public Geometry Geometry { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("reference")]
        public string Reference { get; set; }

        [JsonProperty("place_id")]
        public string PlaceId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("rating")]
        public double Rating { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("reviews")]
        public Review[] Reviews { get; set; }

        [JsonProperty("types")]
        public string[] Types { get; set; }

        [JsonProperty("vicinity")]
        public string Vicinity { get; set; }

        [JsonProperty("website")]
        public string Website { get; set; }
    }

    public class AltId
    {
        [JsonProperty("place_id")]
        public string PlaceId { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }
    }

    public class AddressComponent
    {
        [JsonProperty("short_name")]
        public string ShortName { get; set; }

        [JsonProperty("long_name")]
        public string LongName { get; set; }

        [JsonProperty("types")]
        public string[] Types { get; set; }
    }

    public class Geometry
    {
        [JsonProperty("location")]
        public Location Location { get; set; }

        [JsonProperty("viewport")]
        public Viewport Viewport { get; set; }
    }

    public class Location
    {
        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lng")]
        public double Lng { get; set; }
    }

    public class Viewport
    {
        [JsonProperty("northeast")]
        public Location Northeast { get; set; }

        [JsonProperty("southwest")]
        public Location Southwest { get; set; }
    }

    public class Review
    {
        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("author_name")]
        public string AuthorName { get; set; }

        [JsonProperty("aspects")]
        public Aspect[] Aspects { get; set; }

        [JsonProperty("author_url")]
        public string AuthorUrl { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("rating")]
        public long Rating { get; set; }

        [JsonProperty("time")]
        public long Time { get; set; }
    }

    public class Aspect
    {
        [JsonProperty("rating")]
        public long Rating { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

  
}
