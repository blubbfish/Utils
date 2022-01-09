using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BlubbFish.Utils {
  public class HttpEndpoint {
    private readonly HttpClient client = new HttpClient();
    private readonly String server = "";

    public enum RequestMethod {
      GET,
      POST,
      PUT
    }

    public HttpEndpoint(String server, String auth = null) {
      this.server = server;
      if(auth != null) {
        this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(auth);
      }
    }

    public HttpEndpoint(String server, (String scheme, String parameter) auth) {
      this.server = server;
      this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(auth.scheme, auth.parameter);
    }

    public async Task<String> RequestString(String address, String json = "", Boolean withoutput = true, RequestMethod method = RequestMethod.GET) {
      String ret = null;
      try {
        HttpResponseMessage response = null;
        if(method == RequestMethod.POST || method == RequestMethod.PUT) {
          HttpContent content = new StringContent(json);
          content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
          //content.Headers.Add("Content-Type", "application/json");
          if(method == RequestMethod.POST) {
            response = await this.client.PostAsync(this.server + address, content);
          } else if(method == RequestMethod.PUT) {
            response = await this.client.PutAsync(this.server + address, content);
          }
          content.Dispose();
        } else if(method == RequestMethod.GET) {
          response = await this.client.GetAsync(this.server + address);
        }
        if(!response.IsSuccessStatusCode) {
          throw new Exception(response.StatusCode + ": " + response.ReasonPhrase);
        }
        if(withoutput && response != null) {
          ret = await response.Content.ReadAsStringAsync();
        }
      } catch(Exception e) {
        throw new WebException(String.Format("Error while opening resource: \"{0}\" Method {1} Data {2} Fehler {3}", this.server + address, method, json, e.Message));
      }
      return ret;
    }
  }
}
