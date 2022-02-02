export const AppUrl = 'https://localhost:7265/api';

export const Api : IApi = {
    get: url => {
        return fetch(url).then(response => response.json());
    }
}

interface IApi {
    get: <T>(url: string) => Promise<T>;
}
