import dayjs from "dayjs";

export const dateFormat = "DD.MM.YYYY";

export function formatDate(date: Date) {
    const dayjsDate = dayjs(date);
    const template = "DD.MM.YYYY";
    return dayjsDate.format(template);
}

export function formatDateTime(date: Date | null): string {
    const dayjsDate = dayjs(date);
    const template = "DD.MM.YYYY HH:mm:ss";
    return dayjsDate.format(template);
}
