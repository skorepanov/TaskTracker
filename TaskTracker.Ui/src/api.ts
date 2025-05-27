export const AppUrl = "https://localhost:7265/api";

export const Api : IApi = {
    get: async (url: string) => {
        const response = await fetch(url);
        return await response.json();
    },

    post: async (url: string, params: object) => {
        const response = await fetch(url, {
            headers,
            method: "POST",
            body: JSON.stringify(params),
        });

        return await response.json();
    },

    put: async (url: string, params: object) => {
        const response = await fetch(url, {
            headers,
            method: "PUT",
            body: JSON.stringify(params),
        });

        return await response.json();
    },

    delete: async (url: string) => {
        await fetch(url, {
            headers,
            method: "DELETE"
        });
    }
}

interface IApi {
    get: <T>(url: string) => Promise<T>;
    post: <T>(url: string, params: object) => Promise<T>;
    put: <T>(url: string, params: object) => Promise<T>;
    delete: (url: string) => Promise<void>;
}

const headers = {
    Accept: "application/json",
    "Content-Type": "application/json",
};
