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
    /// GooglePlacesModel represents the deserialized JSON returned by GooglePlaces API
    /// Does not contains coordinates, used to address autocomplete
    /// </summary>

    public class GooglePlacesModel
    {
        [JsonProperty("predictions")]
        public Prediction[] Predictions { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public class Prediction
    {
        [JsonProperty("place_id")]
        public string PlaceId { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("matched_substrings")]
        public MatchedSubstring[] MatchedSubstrings { get; set; }

        [JsonProperty("structured_formatting")]
        public StructuredFormatting StructuredFormatting { get; set; }

        [JsonProperty("reference")]
        public string Reference { get; set; }

        [JsonProperty("terms")]
        public Term[] Terms { get; set; }

        [JsonProperty("types")]
        public string[] Types { get; set; }
    }

    public class MatchedSubstring
    {
        [JsonProperty("length")]
        public long Length { get; set; }

        [JsonProperty("offset")]
        public long Offset { get; set; }
    }

    public class StructuredFormatting
    {
        [JsonProperty("main_text_matched_substrings")]
        public MatchedSubstring[] MainTextMatchedSubstrings { get; set; }

        [JsonProperty("main_text")]
        public string MainText { get; set; }

        [JsonProperty("secondary_text")]
        public string SecondaryText { get; set; }
    }

    public class Term
    {
        [JsonProperty("offset")]
        public long Offset { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }


}
