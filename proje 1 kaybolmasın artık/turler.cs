using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proje_1_kaybolmasın_artık
{
    class turler
    {
        private double _cyu;
        private double _cyg;
        private double _tyu;
        private double _tyg;
        private string _tur;


        public turler(double canakuzunluk,double canakuzgenislik, double tacuzunluk   ,double tacgenislik,string cins)
        {
            _cyu = canakuzunluk;
            _cyg = canakuzgenislik;
            _tyu = tacuzunluk;
            _tyg = tacgenislik;
            _tur = cins;


        }
        public double Cyu
        {
            get
            {
                return this._cyu;
            }
            set
            {
                this._cyu = value;
            }
        }
        public double Cyg
        {
            get
            {
                return this._cyg;
            }
            set
            {
                this._cyg = value;
            }
        }
        public double Tyu
        {
            get
            {
                return this._tyu;
            }
            set
            {
                this._tyu = value;
            }
        }
        public double Tyg
        {
            get
            {
                return this._tyg;
            }
            set
            {
                this._tyg = value;
            }
        }
        public string Tur
        {
            get
            {
                return this._tur;
            }
            set
            {
                this._tur = value;
            }
        }
      

    }
}
