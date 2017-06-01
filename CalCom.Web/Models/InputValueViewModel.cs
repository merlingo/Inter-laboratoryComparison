using Calcom.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalCom.Web.Models
{
    //th ler için baslik PD listesi oluşturulur. 
    //editor template oluşturulmalı
    public class InputValueViewModel
    {
        public int olcId { get; set; }
        public List<String> baslikPD;
        public List<string> olcumNoktasi;

        public InputValueViewModel(int id)
        {
            olcId = id;
            baslikPD = new List<String>();
            olcumNoktasi = new List<string>();

        }
        public int getOlcId()
        {
            return olcId;
        }
    }
        //public class SquareInputValueViewModel : InputValueViewModel
        //{
        //    public OlcumSonuc[,] sonuclar;
        //    private int? nullable;
        //    private int p1;
        //    private int p2;
        //    /*
        //     * id--> karşılaştırma id, 
        //     * b--> olcum sonuc row sayısı - parametre sayısı
        //     * o-->olcum sonuc colum sayısı - olcum noktası sayısı
        //     */ 
        //    public SquareInputValueViewModel(int id, int b, int o) : base(id)
        //    {
        //        sonuclar = new OlcumSonuc[b,o];
        //    }
           
          
        //}

        public class NestedInputValueViewModel : InputValueViewModel
        { 
            public OlcumSonuclari[] sonuclar;
            public int[] olcumNoktaSayilari;//her parametre Degeri için farklı sayıda ölçüm noktası vardır. Bu değişimin kontrolü için kullanılacak
            public NestedInputValueViewModel(int id, int b, int[] noktaSayilari)
                : base(id)
            {
                sonuclar = new OlcumSonuclari[b];

                olcumNoktaSayilari = noktaSayilari;
            }
        }

    
}