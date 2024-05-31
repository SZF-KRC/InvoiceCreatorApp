﻿using InvoiceCreatorApp.MVVM;
using System;

namespace InvoiceCreatorApp.Models
{
    public class Invoice : ViewModelBase
    {
        private string _description;
        private string _numberOfGoods;
        private string _pricePerPiece;
        private int _position;

        private string _customerName;
        private string _customerCity;
        private string _customerStreet;   
        private string _customerPostalCode;
        private string _customerNumber;


        /// <summary>
        /// Firmenname der Rechnung
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Stadt des Unternehmens
        /// </summary>
        public string CompanyCity { get; set; }

        /// <summary>
        /// Straße des Unternehmens
        /// </summary>
        public string CompanyStreet { get; set; }

        /// <summary>
        /// Postleitzahl des Unternehmens
        /// </summary>
        public string CompanyPostalCode { get; set; }

        /// <summary>
        /// Position des Postens in der Rechnung
        /// </summary>
        public int Position
        {
            get { return _position; }
            set { _position = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Konstante Steuer von 20%
        /// </summary>
        public const double Tax = 0.2; // Tax 20%

        /// <summary>
        /// Kundenname der Rechnung
        /// </summary>
        public string CustomerName
        {
            get { return _customerName; }
            set { _customerName = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Stadt des Kunden
        /// </summary>
        public string CustomerCity
        {
            get { return (_customerCity); }
            set { _customerCity = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Straße des Kunden
        /// </summary>
        public string CustomerStreet
        {
            get { return _customerStreet; }
            set { _customerStreet = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Postleitzahl des Kunden
        /// </summary>
        public string CustomerPostalCode
        {
            get { return _customerPostalCode; }
            set { _customerPostalCode = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Kundennummer der Rechnung
        /// </summary>
        public string CustomerNumber
        {
            get { return _customerNumber; }
            set { _customerNumber = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Beschreibung der Waren in der Rechnung
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Anzahl der Waren in der Rechnung
        /// </summary>
        public string NumberOfGoods
        {
            get { return _numberOfGoods; }
            set { _numberOfGoods = value; OnPropertyChanged(); OnPropertyChanged(nameof(NettoPrice)); }
        }

        /// <summary>
        /// Preis pro Stück der Waren in der Rechnung
        /// </summary>
        public string PricePerPiece
        {
            get { return _pricePerPiece; }
            set { _pricePerPiece = value; OnPropertyChanged(); OnPropertyChanged(nameof(NettoPrice)); }
        }

        /// <summary>
        /// Berechnet die Steuer für die Waren
        /// </summary>
        /// <returns>Steuerbetrag</returns>
        public double CalculationTax() => (Int32.Parse(NumberOfGoods) * (Double.Parse(PricePerPiece)) * Tax);

        /// <summary>
        /// Berechnet den Endpreis inklusive Steuer für die Waren
        /// </summary>
        /// <returns>Endpreis inklusive Steuer</returns
        public double CalculationFinalPrice() => (Int32.Parse(NumberOfGoods) * (Double.Parse(PricePerPiece)) * (1 + Tax));

        /// <summary>
        /// Berechnet den Gesamtpreis ohne Steuer für die Waren
        /// </summary>
        /// <returns>Gesamtpreis ohne Steuer</returns>
        public double TotalPrice() => (Int32.Parse(NumberOfGoods) * (Double.Parse(PricePerPiece)));

        /// <summary>
        /// Berechnet den Nettopreis der Waren
        /// </summary>
        public double NettoPrice => (Int32.Parse(NumberOfGoods) * (Double.Parse(PricePerPiece)));

        /// <summary>
        /// Konstruktor für die Klasse Invoice
        /// </summary>
        public Invoice() { }
    }
}
