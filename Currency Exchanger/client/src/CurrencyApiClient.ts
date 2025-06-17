import type { Currency, CurrencyPrice } from './types';

export class CurrencyApiClient {
  private baseUrl: string;

  constructor(baseUrl: string = 'http://localhost:5081') {
    this.baseUrl = baseUrl;
  }

  async getAllCurrencies(): Promise<Currency[]> {
    const response = await fetch(`${this.baseUrl}/Currency`);
    if (!response.ok) {
      throw new Error('Failed to fetch currencies');
    }
    return await response.json();
  }

  async getCurrencyByCode(code: string): Promise<Currency> {
    const response = await fetch(`${this.baseUrl}/Currency/${code}`);
    if (!response.ok) {
      throw new Error(`Failed to fetch currency ${code}`);
    }
    return await response.json();
  }

  async getCurrencyPrices(
    paymentCurrency: string,
    purchasedCurrency: string,
    fromDateTime: string,
    toDateTime?: string
  ): Promise<CurrencyPrice[]> {
    let url = `${
      this.baseUrl
    }/Currency/prices/?PaymentCurrency=${paymentCurrency}&PurchasedCurrency=${purchasedCurrency}&FromDateTime=${encodeURIComponent(
      fromDateTime
    )}`;

    if (toDateTime) {
      url += `&ToDateTime=${encodeURIComponent(toDateTime)}`;
    }

    const response = await fetch(url);

    if (!response.ok) {
      throw new Error('Failed to fetch currency prices');
    }
    const data = await response.json();

    return data.map((item: CurrencyPrice) => ({
      ...item,
      datetime: new Date(item.datetime)
    }));
  }
}
