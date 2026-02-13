export const ApiUrl = process.env.REACT_APP_API_URL;

interface IApiResponse<T> {
    isOk: boolean;
    value: T | null;
    error: string | null;
}

export const Api: IApi = {
    get: async <T>(url: string) => {
        try {
            const response = await fetch(url);

            if (response.status === 200) {
                const data: T = await response.json();

                const dataResponse: IApiResponse<T> = {
                    isOk: true,
                    value: data,
                    error: null,
                };

                return dataResponse;
            }

            const errorResponse: IApiResponse<T> = {
                isOk: false,
                error: response.statusText,
                value: null,
            };

            console.error(response.statusText);

            return errorResponse;
        } catch (error) {
            console.error(error);

            const errorResponse: IApiResponse<T> = {
                isOk: false,
                error: "Network request failed",
                value: null,
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

            if (response.status === 200) {
                const data: T = await response.json();

                const dataResponse: IApiResponse<T> = {
                    isOk: true,
                    value: data,
                    error: null,
                };

                return dataResponse;
            }

            const errorResponse: IApiResponse<T> = {
                isOk: false,
                error: response.statusText,
                value: null,
            };

            console.error(response.statusText);

            return errorResponse;
        } catch (error) {
            console.error(error);

            const errorResponse: IApiResponse<T> = {
                isOk: false,
                error: "Network request failed",
                value: null,
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

            if (response.status === 200) {
                const data: T = await response.json();

                const dataResponse: IApiResponse<T> = {
                    isOk: true,
                    value: data,
                    error: null,
                };

                return dataResponse;
            }

            const errorResponse: IApiResponse<T> = {
                isOk: false,
                error: response.statusText,
                value: null,
            };

            console.error(response.statusText);

            return errorResponse;
        } catch (error) {
            console.error(error);

            const errorResponse: IApiResponse<T> = {
                isOk: false,
                error: "Network request failed",
                value: null,
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

            if (response.status === 200) {
                const data: T = await response.json();

                const dataResponse: IApiResponse<T> = {
                    isOk: true,
                    value: data,
                    error: null,
                };

                return dataResponse;
            }

            const errorResponse: IApiResponse<T> = {
                isOk: false,
                error: response.statusText,
                value: null,
            };

            console.error(response.statusText);

            return errorResponse;
        } catch (error) {
            console.error(error);

            const errorResponse: IApiResponse<T> = {
                isOk: false,
                error: "Network request failed",
                value: null,
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
