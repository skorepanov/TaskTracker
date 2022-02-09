export const AppUrl = 'https://localhost:7265/api';

export const Api : IApi = {
    get: url => {
        return fetch(url).then(response => response.json());
    },

    post: (url, params) => {
        return fetch(url, {
                headers,
                method: 'POST',
                body: JSON.stringify(params),
            })
            .then(response => response.json());
    }
}

interface IApi {
    get: <T>(url: string) => Promise<T>;
    post: <T>(url: string, params: object) => Promise<T>;
}

const headers = {
    Accept: 'application/json',
    'Content-Type': 'application/json',
};
