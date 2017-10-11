﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeccApp.Core.Models.Requests
{
    public class AddressModel
    {
        public string AddressName { get; set; }
        public string AddressStreet { get; set; }
        public string AddressProvince { get; set; }
        public string AddressNumber { get; set; }
        public string AddressFloor { get; set; }
        public string AddressHospitalRoom { get; set; }
        public string DisplayAddress { get; set; }
        public string PlaceId { get; set; }

        public double Lat { get; set; }
        public double Lng { get; set; }



        public AddressModel()
        {
            
        }

        public AddressModel(string AddressName,string AddressStreet,string AddressProvince, string AddressNumber, string AddressFloor, string AddressHospitalRoom,string DisplayAddress,string PlaceId,float Lat,float Lng)
        {
            this.AddressName = AddressName;
            this.AddressStreet = AddressStreet;
            this.AddressProvince = AddressProvince;
            this.AddressNumber = AddressNumber;
            this.AddressFloor = AddressFloor;
            this.AddressHospitalRoom = AddressHospitalRoom;
            this.DisplayAddress = DisplayAddress;
            this.PlaceId = PlaceId;
            this.Lat = Lat;
            this.Lng = Lng;
        }

        public AddressModel(string AddressName, string AddressStreet,string AddressProvince, string AddressNumber, string AddressFloor,string DisplayAddress, string PlaceId,double Lat, double Lng)
        {
            this.AddressName = AddressName;
            this.AddressStreet = AddressStreet;
            this.AddressProvince = AddressProvince;
            this.AddressNumber = AddressNumber;
            this.AddressFloor = AddressFloor;
            this.DisplayAddress = DisplayAddress;
            this.PlaceId = PlaceId;
            this.Lat = Lat;
            this.Lng = Lng;
        }

    }
}
