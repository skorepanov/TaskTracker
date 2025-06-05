import dayjs from "dayjs";
import "dayjs/locale/ru";

export function formatDate(date: Date) {
    const dayjsDate = dayjs(date);
    const template = "DD.MM.YYYY";
    return dayjsDate.format(template);
}

export function formatDateTime(date: Date | null): string {
    const dayjsDate = dayjs(date);
    const template = "DD.MM.YYYY HH:mm:ss ([GMT:]Z)";
    return dayjsDate.format(template);
}
