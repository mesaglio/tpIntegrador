using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace tp_integrador.Models
{
    public class SIMPLEX
    {
        private int A1;
        private int A2;
        private string B1 = "MAX";
        private int B2 = 1;
        private int BNmas1 = 0;
        private int Bn = 1;
        private List<ArraySegment<string>> variables;
        private List<ArraySegment<string>> restricciones;
        
        private string[,] cvs; 
        private string API = "https://dds-simplexapi.herokuapp.com/consultar";

        private string zeroforvariable()
        {
            string c = "";
            for (int i = A1; i == 0; i++)
                { c = c + "," + Bn.ToString();}
            return c;
        }

        private string boleano()
        {
            string c = "";
            for (int i = A1; i == 0; i++)
            { c = c + ",TRUE"; }
            return c;
        }

        private void SetA1() => A1 = variables.Count();
        private void SetA2() => A2 = restricciones.Count();
        private string FilaA(){ return A1.ToString() + "," + A2.ToString() + Environment.NewLine;}
        private string FilaB() { return B1 + "," + B2.ToString() + zeroforvariable() + "," + BNmas1.ToString() + Environment.NewLine; }
        private string FikaC() { return boleano(); }

        public SIMPLEX(List<ArraySegment<string>> variables, List<ArraySegment<string>> restricciones)
        {
            this.variables = variables;
            this.restricciones = restricciones;
            SetA1();
            SetA2();
        }

        public string Simplex(string postData)
        {
            WebRequest request = WebRequest.Create(API);
            request.Method = "POST";

            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            // Set the ContentType property of the WebRequest.  
           // request.ContentType = "application/x-www-form-urlencoded";
            // Set the ContentLength property of the WebRequest.  
            request.ContentLength = byteArray.Length;
            // Get the request stream.  
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.  
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.  
            dataStream.Close();
            // Get the response.  
            WebResponse response = request.GetResponse();
            // Display the status.  
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.  
            dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.  
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.  
            string responseFromServer = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            response.Close();
            return responseFromServer;
        }

    }
}