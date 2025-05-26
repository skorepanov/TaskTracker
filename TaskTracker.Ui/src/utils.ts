import dayjs from "dayjs";
import 'dayjs/locale/ru';

export function formatDateTime(date: Date | null): string {
    const dayjsDate = dayjs(date);
    const template = 'DD.MM.YYYY HH:mm:ss ([GMT:]Z)';
    return dayjsDate.format(template);
}
