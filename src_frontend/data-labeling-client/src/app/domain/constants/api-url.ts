import { environment } from "src/environments/environment";

export namespace ApiUrl {
    // Data
    export const labelData = () => environment.apiUrl + 'data/label-data/'
    export const uploadRawData = () => environment.apiUrl + 'data/raw-data/'
    export const getDataForLabeling = (orderId: number) => environment.apiUrl + 'data/data-for-labeling/' + orderId
    export const getImageForLabeling = (orderId: number) => environment.apiUrl + 'data/image-for-labeling/' + orderId
    export const getCompletedDataSet = (orderId: number, skip: number, take: number, performerId: number | string) =>
        environment.apiUrl + `completed-dataset?orderId=${orderId}&skip=${skip}&take=${take}&performerId=${performerId}`
    export const downloadDataSet = (orderId: number) => environment.apiUrl + `data/completed-dataset-zip/${orderId}`
    export const getDatasetForReview = (orderId: number, performerId: number) => environment.apiUrl + `data/data-for-review/${orderId}/${performerId}`

    // Order
    export const getOrderPrice = (datSetRequiredCount: number, type: string, priority: string, deadline: string) =>
        environment.apiUrl + `order/price?datSetRequiredCount=${datSetRequiredCount}&type=${type}&priority=${priority}&deadline=${deadline}`
    
    export const createOrder = () => environment.apiUrl + 'order/'
    export const cancelOrder = (orderId: number) => environment.apiUrl + `order/cancel/${orderId}`
    export const getProgress = (orderId: number) => environment.apiUrl + `order/progress/${orderId}`
    export const refreshProgress = (orderId: number) => environment.apiUrl + `order/refresh-progress/${orderId}`
    export const getOrderForCustomer = (orderId: number) => environment.apiUrl + `order/for-customer/${orderId}`
    export const getOrderForPerformer = (orderId: number) => environment.apiUrl + `order/for-performer/${orderId}`
    export const getOrdersForCustomer = (orderType: string | null,
                                         minPriority: string | null,
                                         maxPriority: string | null,
                                         isCompleted: boolean | string | null,
                                         isCanceled: boolean | string | null) => environment.apiUrl + 'order/for-customer' + 
                                    `?orderType=${orderType}
                                     &minPriority=${minPriority}
                                     &maxPriority=${maxPriority}
                                     &isCompleted=${isCompleted}
                                     &isCanceled=${isCanceled}`
    
    export const getOrdersForPerformer = (orderType: string | null) => environment.apiUrl + `order/for-performer?orderType=${orderType}`
    export const getAllowedPriorities = () => environment.apiUrl + 'order/allowed-priorities/'
    export const getOrderPerformers = (orderId: number) => environment.apiUrl + `order/performers/${orderId}`

    // Review
    export const addReview = () => environment.apiUrl + 'review/'

    // Auth
    export const login = () => environment.apiUrl + 'user/login/'
    export const register = () => environment.apiUrl + 'user/register/'

    // User
    export const getCustomerStatistic = () => environment.apiUrl + 'user/customer-statistic/'
    export const getPerformerStatistic = () => environment.apiUrl + 'user/performer-statistic/'
    export const getPerformerInfo = () => environment.apiUrl + 'user/performer-info/';

    // payment
    export const getMoney = () => environment.apiUrl + 'payment/withdraw-money/';
    export const confirmPay = (orderId: number) => environment.apiUrl + `payment/confirm/${orderId}`;
    export const getCustomerHistory = () => environment.apiUrl + `payment/customer-history/`;
    export const getPerformerHistory = () => environment.apiUrl + `payment/performer-history/`;
    export const getLiqPayData = (orderId: number) => environment.apiUrl + `payment/liqpay-data/${orderId}`
}