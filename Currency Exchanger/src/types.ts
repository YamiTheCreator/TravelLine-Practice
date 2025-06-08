export interface Currency {
  code: string;
  name: string;
  description: string;
  symbol: string;
  minPrice?: number;
  maxPrice?: number;
}

export interface CurrencyPrice {
  id: number;
  currencyCode: string;
  price: number;
  datetime: Date;
  currency: Currency;
}

export interface QuickFilter {
  id: string;
  name: string;
  paymentCurrency: string;
  purchaseCurrency: string;
}
