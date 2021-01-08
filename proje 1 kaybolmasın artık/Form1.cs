using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace proje_1_kaybolmasın_artık
{
    public partial class Form1 : Form
    {

        ArrayList arrayl = new ArrayList();
        string[,] TestVerileri = new string[30,5];
        ArrayList digerVeri = new ArrayList();
        
        public Form1()
        {
            

            string dosya_yolu = "iris.txt";
            FileStream fs = new FileStream(dosya_yolu, FileMode.Open, FileAccess.Read);
            StreamReader sw = new StreamReader(fs);
            string yazi = sw.ReadLine();
            
            
            
            while (yazi != null)
            {

                string temp="" ;
                string[] t = new string[5];
                int sayac = 0;
                int sayac2 = 0;
               foreach(char c in yazi)
                {
                    if(!c.Equals(' ')) { 
                    if (','.Equals(c))
                    {
                        t[sayac2] = temp;
                        
                        temp = "";
                        sayac2++;
                    }
                    else
                    {
                        temp += c;
                    }
                    }
                    sayac++;
                }
                t[sayac2] = temp;
                arrayl.Add(new turler(Convert.ToDouble(t[0]), Convert.ToDouble(t[1]), Convert.ToDouble(t[2]), Convert.ToDouble(t[3]), t[4]));
                    sayac2++;
                    sayac++;

                yazi = sw.ReadLine();
            }//text dosyasındaki veri ile  turler clasından oluşan objeler oluşturur ve bir arrayliste atar
                int s0 = 0;
                int s1 = 0;

            foreach(turler t in arrayl)//başarı oranı hesaplaması için testverilerinin sınıflandırılması
            {
               
               
                if ((s0 > 39 && s0 < 50) || (s0 > 89 && s0 < 100) || (s0 > 139 && s0 < 150))//TEst verisinin ayrıştırılması
                {
                    TestVerileri[s1,0]=t.Cyu.ToString();
                    TestVerileri[s1, 1] =t.Cyg.ToString();
                    TestVerileri[s1, 2] = t.Tyu.ToString();
                    TestVerileri[s1, 3] = t.Tyg.ToString();
                    TestVerileri[s1, 4] = t.Tur;
                    s1++;
                    
                }
                else
                {
                    
                    digerVeri.Add(t);
                }
                s0++;

            }
          
            sw.Close();
            fs.Close();
            
            InitializeComponent();
            textBox1.Text = "0";
            textBox2.Text = "0";
            textBox3.Text = "0";
            textBox4.Text = "0";
            textBox5.Text = "0";
            textBox6.Text = "0";
            goruntule(arrayl);
        }

    

        string enyakin;

        public string[,] knn(int k,double f1,double f2,double f3,double f4,ArrayList veri,out string enyakin)
        {
            string[,] value = new string[k,2];
            turler[] closest = new turler[k];//en yakın turun objesini tutar
            double[] distances = new double[k];// en yakın tür objelerinin uzaklıklarını tutar
            
            for(int i = 0; i < k; i++)//uzaklık dizisine  ilk k tane atama gerçekleşebilmesi için yüksek bir değer verdim
            {
                distances[i] = 100;
            }


            Dictionary<String, int> dic = new Dictionary<string, int>()     //k tane en yakın eleman için;
            {                                                               //0.indiste I.setosa,1.indiste I.versicolor,2.indiste virginica sayısını tutar
                {"Iris-setosa",0 },
                {"Iris-versicolor",0},
                {"Iris-virginica",0 }
            };

            foreach (turler t in veri)//knn ile test edilmek istenen çiçeği veri arrayyinin içerisindeki çiçeklerle karşılaştırır
            {
                double d1 = 0;
                double d2 = 0;
                double d3 = 0;
                double d4 = 0;
                double distance = 0;

                if (f1 != 0)// eğer girilen çiçeğin bir değeri 0 değil ise uzaklık formülünü uygular,(eğer bir değer 0 olmuşsa o değere sahip yaprak yoktur)
                    d1 =Math.Pow(f1 - t.Cyu,2);
                if (f2 != 0)
                    d2 = Math.Pow(f2 - t.Cyg, 2);
                if (f3 != 0)
                    d3 = Math.Pow(f3 - t.Tyu, 2);
                if (f4 != 0)
                    d4 = Math.Pow(f4 - t.Tyg, 2);

                distance = Math.Sqrt(d1 + d2 + d3 + d4);

                for(int i = 0; i < k; i++)//k elemanlı distances dizisi içinde gezer
                {
                    if (distances[i] > distance)//eğer distances dizisinin içindekilerden daha küçük bir uzaklıkta çiçek bulunmuşsa
                    {
                        for (int j = k-1; j > i; j--)//distances dizisindekilerin herhangibirinden küçük uzaklığa sahip çiçeği uygun aralığa eklerdiğerlerini sona kaydırır(yani listedeki en uzak çiçek silinir)
                        {
                            distances[j] = distances[j-1];
                            closest[j] = closest[j-1];
                        }
                        distances[i] = distance;
                        closest[i] = t;
                        break;
                    }
                    
                }

              
            }
            for (int i = 0; i < k; i++)//returnlanacak diziye en yakın k kadar verinin atılması
            {

                value[i, 0] = Math.Round(distances[i], 4).ToString();
                value[i, 1] = closest[i].Tur;
                dic[closest[i].Tur]++;
  
                
            }
            
            string maxindex= "Iris-setosa";//default bir değer atanamsı
           
            
            foreach(KeyValuePair<string,int> d in dic)
            {
                if (d.Value > dic[maxindex])
                {
                    maxindex = d.Key;// en çok tekrarı bulma
                }
                
            }
            enyakin = maxindex;//global değişkene geçici olarak knn nin sonucu atanır,knn tekrar çaırıldığında global enyakin değişkeni tekrar atanmak zorundadır
            foreach (KeyValuePair<string, int> d in dic)
            {
                
                if (d.Value == dic[maxindex]&&!d.Key.Equals(maxindex))// eğer en çok tekrar edenlerin sayısında eşitlik var ise..
                {
                    /*  
                        program eşitlik olma durumda direk en yakın çiçeği almaktansa eşitlik olan çiçekler arasındaki en yakın çiçeği seçer
                     */
                    double distance = 100;
                    for(int i =0; i < k; i++)
                    {
                        if ((d.Key==closest[i].Tur || maxindex==closest[i].Tur )&& distances[i] < distance)
                        {
                            enyakin = closest[i].Tur;
                            distance = distances[i];
                        }

                    }

                }

            }

            return value;
        }



        private void button1_Click(object sender, EventArgs e)//bitki sınıflandırması butonunun knnyi çağırıp değerleri tabloya ataması
        {
        if(!(textBox1.Text=="0"&& textBox2.Text == "0" && textBox3.Text == "0" && textBox4.Text == "0" && textBox5.Text == "0")) {
                dataGridView2.Rows.Clear();
            string[,] s=knn(Convert.ToInt32(textBox1.Text), Convert.ToDouble(textBox2.Text), Convert.ToDouble(textBox3.Text), Convert.ToDouble(textBox4.Text), Convert.ToDouble(textBox5.Text),arrayl,out enyakin);
            string[] s2 = new string[2];
            for(int i = 0; i < s.Length/2; i++)
            {
                s2[0] = s[i, 0];
                s2[1] = s[i, 1];
                dataGridView2.Rows.Add(s2);
            }
            label6.Text = enyakin;
                for (int i = 0; i < this.dataGridView2.Rows.Count; i++)//satır sayısını yazdırma
                {
                    this.dataGridView2.Rows[i].HeaderCell.Value = (i + 1).ToString() + ".";
                }
            }
        else
        {
                label6.Text = "Lütfen k'ya değer girin";
            }

        }



        string[,,] basariOraniTablosu;
        /*
         başarı oranı için yapılan hesaplamalar başarıOraniTablosu 3 boyutlu dizisinde tutulur,30,k,2 uzunluklarındadır,30 adet test verisinin ;k adet komşusunun ,
         2 adet bilgisini(knn sonucu yani uzaklığı ve türün ismi) tutar
             */
        string[] tahminler;

        public void basariHesapla()// 30 adet test verisini kalan 120 veri içerisinden geçirir ve gerekli bilgileri basariOraniTablosu listesinde tutar
        {
            double oran = 0;
            basariOraniTablosu = new string[30, Convert.ToInt32(textBox6.Text), 2];
            tahminler = new string[30];
            for (int i = 0; i < 30; i++)//her test verisi için tekrar çağırılır
            {
                    enyakin = "null";
                string[,] s = new string[Convert.ToInt32(textBox6.Text), 2];
                s= (knn(Convert.ToInt32(textBox6.Text), Convert.ToDouble(TestVerileri[i, 0]), Convert.ToDouble(TestVerileri[i, 1]), Convert.ToDouble(TestVerileri[i, 2]), Convert.ToDouble(TestVerileri[i, 3]), digerVeri, out enyakin));

                for(int j=0;j< Convert.ToInt32(textBox6.Text);j++)
                {
                    basariOraniTablosu[i, j, 0] = s[j, 0];
                    basariOraniTablosu[i, j, 1] = s[j, 1];
                }
              
                if (enyakin.Equals(TestVerileri[i, 4]))
                {
                    oran++;
                }
                tahminler[i] = enyakin;
                
            }

            label12.Text = ("  %" + oran / 30 * 100 + "Başarılı");
        }
        private void button2_Click(object sender, EventArgs e)//knn metodunu bütün verileri içeren arraylist  ile değil,test verilerini içermeyen bir arraylist ile çağırır,(başarıhesapla metodu)
        {
            basariHesapla();
            for (int i = 0; i <30; i++)
            {
                Console.WriteLine(TestVerileri[i, 4]+"  "+ tahminler[i]);

            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)//başarı oranı hesaplandıktan sonra 30 adet veriden istenen verinin komşularını ve özelliklerini gösterir
        {
            label13.Text = "";
            label14.Text = "";
            dataGridView3.Rows.Clear();
            string[] row = new string[2];
            if (textBox6.Text.Equals("0"))
            {
                dataGridView3.Rows.Add("Önce Bir Şeyler Hesaplayın!");
            }
            else
            {

            
            for(int i = 0; i < Convert.ToInt32(textBox6.Text); i++)
            {
                row[0] = basariOraniTablosu[Convert.ToInt32(comboBox1.Text), i, 0];
                row[1] = basariOraniTablosu[Convert.ToInt32(comboBox1.Text), i, 1];
                dataGridView3.Rows.Add(row);
            }
                label13.Text = TestVerileri[Convert.ToInt32(comboBox1.Text), 4];
                label14.Text = tahminler[Convert.ToInt32(comboBox1.Text)];

        }}

        public void goruntule(ArrayList array)//velirtilen veriyi dataGridView e atar
        {
            string[] row = new string[5];

            foreach(turler i in array)
            {
                row[0]= i.Cyu.ToString();
                row[1] = i.Cyg.ToString();
                row[2] = i.Tyu.ToString();
                row[3] = i.Tyg.ToString();
                row[4] = i.Tur;

                dataGridView1.Rows.Add(row);

            }
            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)//satır sayısını yazdırma
            {
                this.dataGridView1.Rows[i].HeaderCell.Value = (i + 1).ToString() + ".";
            }

        }

        private void button3_Click(object sender, EventArgs e)//diziye veri ekler
        {
            label18.Text = "";
            if (textBox10.Text == "" || textBox9.Text == "" || textBox8.Text == "" || textBox7.Text == "" || textBox11.Text == "" )
            {
                label18.Text = "Lütfen Bütün Boşlukları Doldurun!!";
            }
            else
            {
                arrayl.Add(new turler(Convert.ToDouble(textBox10.Text), Convert.ToDouble(textBox9.Text), Convert.ToDouble(textBox9.Text), Convert.ToDouble(textBox7.Text), textBox11.Text));
                label18.Text = "";
            }
           }

        private void button6_Click(object sender, EventArgs e)//yenile butonu
        {
            label18.Text = "";
            dataGridView1.Rows.Clear();
            goruntule(arrayl);
        }

        private void button4_Click(object sender, EventArgs e)//belirtilen insis siler
        {
            label18.Text = "";

            try
            {
                arrayl.Remove(arrayl[Convert.ToInt32(textBox12.Text)]);
            }
            catch (ArgumentOutOfRangeException)
            {
                
                
                    label18.Text = "index değeri en fazla son satır numarasının 1 eksiği kadar olabilir";
               
            }
            catch (FormatException)
            {


                label18.Text = "İndex değeri 0 ile  son satır numarasının 1 eksiği arasında değer alabilir!";

            }


        }

        private void button5_Click(object sender, EventArgs e)//bütün veriyi siler
        {
            label18.Text = "";
            arrayl.Clear();
        }
    }

}
