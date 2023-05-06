import { DatePipe } from "@angular/common";

export const convertToISOFormat = (date, datePipe: DatePipe): string => {
    return datePipe.transform(date, 'yyyy-MM-ddT00:00:00.000').concat('Z');
};