export const AppUrl = "https://localhost:7265/api";

interface IApiResponse<T> {
    isOk: boolean;
    result: T | null;
    error: string | null;
}

export const Api: IApi = {
    get: async <T>(url: string) => {
        try {
            const response = await fetch(url);
            const data: IApiResponse<T> = await response.json();

            if (!data.isOk) {
                console.error(data.error);
            }

            return data;
        } catch (error) {
            console.error(error);

            const errorResponse: IApiResponse<T> = {
                isOk: false,
                error: "Network request failed",
                result: null,
            };

            return errorResponse;
        }
    },

    post: async <T>(url: string, params: object) => {
        try {
            const response = await fetch(url, {
                headers,
                method: "POST",
                body: JSON.stringify(params),
            });

            const data: IApiResponse<T> = await response.json();

            if (!data.isOk) {
                console.error(data.error);
            }

            return data;
        } catch (error) {
            console.error(error);

            const errorResponse: IApiResponse<T> = {
                isOk: false,
                error: "Network request failed",
                result: null,
            };

            return errorResponse;
        }
    },

    put: async <T>(url: string, params: object) => {
        try {
            const response = await fetch(url, {
                headers,
                method: "PUT",
                body: JSON.stringify(params),
            });

            const data: IApiResponse<T> = await response.json();

            if (!data.isOk) {
                console.error(data.error);
            }

            return data;
        } catch (error) {
            console.error(error);

            const errorResponse: IApiResponse<T> = {
                isOk: false,
                error: "Network request failed",
                result: null,
            };

            return errorResponse;
        }
    },

    delete: async <T>(url: string) => {
        try {
            const response = await fetch(url, {
                headers,
                method: "DELETE",
            });

            const data: IApiResponse<T> = await response.json();

            if (!data.isOk) {
                console.error(data.error);
            }

            return data;
        } catch (error) {
            console.error(error);

            const errorResponse: IApiResponse<T> = {
                isOk: false,
                error: "Network request failed",
                result: null,
            };

            return errorResponse;
        }
    },
};

interface IApi {
    get: <T>(url: string) => Promise<IApiResponse<T>>;
    post: <T>(url: string, params: object) => Promise<IApiResponse<T>>;
    put: <T>(url: string, params: object) => Promise<IApiResponse<T>>;
    delete: <T>(url: string) => Promise<IApiResponse<T>>;
}

const headers = {
    Accept: "application/json",
    "Content-Type": "application/json",
};
