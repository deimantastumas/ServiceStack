using System;
using System.Net;
using System.Threading.Tasks;

namespace ServiceStack
{
	public interface IRestClientAsync : IDisposable
	{
		void SetCredentials(string userName, string password);

        Task<TResponse> GetAsync<TResponse>(IReturn<TResponse> requestDto);
        Task<TResponse> GetAsync<TResponse>(object requestDto);
        Task<TResponse> GetAsync<TResponse>(string relativeOrAbsoluteUrl);
        Task<HttpWebResponse> GetAsync(IReturnVoid requestDto);

        Task<TResponse> DeleteAsync<TResponse>(IReturn<TResponse> requestDto);
        Task<TResponse> DeleteAsync<TResponse>(object requestDto);
        Task<TResponse> DeleteAsync<TResponse>(string relativeOrAbsoluteUrl);
        Task<HttpWebResponse> DeleteAsync(IReturnVoid requestDto);

        Task<TResponse> PostAsync<TResponse>(IReturn<TResponse> requestDto);
        Task<TResponse> PostAsync<TResponse>(object requestDto);
        Task<TResponse> PostAsync<TResponse>(string relativeOrAbsoluteUrl, object request);
        Task<HttpWebResponse> PostAsync(IReturnVoid requestDto);

        Task<TResponse> PutAsync<TResponse>(IReturn<TResponse> requestDto);
        Task<TResponse> PutAsync<TResponse>(object requestDto);
        Task<TResponse> PutAsync<TResponse>(string relativeOrAbsoluteUrl, object request);
        Task<HttpWebResponse> PutAsync(IReturnVoid requestDto);

        Task<TResponse> CustomMethodAsync<TResponse>(string httpVerb, IReturn<TResponse> requestDto);
        Task<TResponse> CustomMethodAsync<TResponse>(string httpVerb, object requestDto);
        Task<HttpWebResponse> CustomMethodAsync(string httpVerb, IReturnVoid requestDto);

        void CancelAsync();
	}

}