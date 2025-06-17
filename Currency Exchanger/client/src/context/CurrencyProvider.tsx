import { useState, useEffect, ReactNode } from 'react';
import { CurrencyApiClient } from '../CurrencyApiClient';
import { Currency, CurrencyPrice, QuickFilter } from '../types';
import CurrencyContext from './CurrencyContext';

const apiClient = new CurrencyApiClient();

const CurrencyProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
  const [currencies, setCurrencies] = useState<Currency[]>([]);
  const [currency, setCurrency] = useState({ paymentCurrency: '', purchaseCurrency: '' });
  const [amount, setAmount] = useState(1);
  const [exchangeRate, setExchangeRate] = useState<CurrencyPrice | null>(null);
  const [loading, setLoading] = useState({
    initial: true,
    rate: false,
    details: false
  });
  const [error, setError] = useState<string | null>(null);
  const [showDetails, setShowDetails] = useState(false);
  const [currencyDetails, setCurrencyDetails] = useState<{
    payment: Currency | null;
    purchase: Currency | null;
  }>({ payment: null, purchase: null });

  const [quickFilters, setQuickFilters] = useState<QuickFilter[]>([]);

  const addQuickFilter = () => {
    if (!currency.paymentCurrency || !currency.purchaseCurrency) return;

    const newFilter: QuickFilter = {
      id: Date.now().toString(),
      name: `${currency.paymentCurrency} / ${currency.purchaseCurrency}`,
      paymentCurrency: currency.paymentCurrency,
      purchaseCurrency: currency.purchaseCurrency
    };

    setQuickFilters((prev) => {
      const updatedFilters = [...prev, newFilter];
      localStorage.setItem('currencyQuickFilters', JSON.stringify(updatedFilters));
      return updatedFilters;
    });
  };

  const removeQuickFilter = (id: string) => {
    setQuickFilters((prev) => {
      const updatedFilters = prev.filter((filter) => filter.id !== id);
      localStorage.setItem('currencyQuickFilters', JSON.stringify(updatedFilters));
      return updatedFilters;
    });
  };

  const applyQuickFilter = (filter: QuickFilter) => {
    setCurrency({
      paymentCurrency: filter.paymentCurrency,
      purchaseCurrency: filter.purchaseCurrency
    });
  };

  useEffect(() => {
    const loadCurrencies = async () => {
      try {
        setError(null);
        setLoading((prev) => ({ ...prev, initial: true }));
        const data = await apiClient.getAllCurrencies();
        setCurrencies(data);
        if (data.length > 1) {
          setCurrency({ paymentCurrency: data[0].code, purchaseCurrency: data[1].code });
        }
      } catch (err) {
        setError(err instanceof Error ? err.message : 'Failed to load currencies');
      } finally {
        setLoading((prev) => ({ ...prev, initial: false }));
      }
    };
    loadCurrencies();
  }, []);

  useEffect(() => {
    if (!currency.paymentCurrency || !currency.purchaseCurrency) return;

    const fetchData = async () => {
      try {
        setLoading((prev) => ({ ...prev, rate: true, details: true }));
        setError(null);

        const now = new Date();
        const yesterday = new Date(now.getTime() - 24 * 60 * 60 * 1000);
        const prices = await apiClient.getCurrencyPrices(
          currency.paymentCurrency,
          currency.purchaseCurrency,
          yesterday.toISOString(),
          now.toISOString()
        );

        if (prices.length > 0) {
          setExchangeRate(prices[prices.length - 1]);
        } else {
          throw new Error('No exchange rate data available');
        }

        const [paymentDetails, purchaseDetails] = await Promise.all([
          apiClient.getCurrencyByCode(currency.paymentCurrency),
          apiClient.getCurrencyByCode(currency.purchaseCurrency)
        ]);

        setCurrencyDetails({
          payment: paymentDetails,
          purchase: purchaseDetails
        });
      } catch (err) {
        setError(err instanceof Error ? err.message : 'Failed to fetch data');
        setExchangeRate(null);
      } finally {
        setLoading((prev) => ({ ...prev, rate: false, details: false }));
      }
    };

    fetchData();
    const intervalId = setInterval(fetchData, 10000);

    return () => clearInterval(intervalId);
  }, [currency]);

  const toggleDetails = () => {
    setShowDetails(!showDetails);
  };

  useEffect(() => {
    const savedFilters = localStorage.getItem('currencyQuickFilters');
    if (savedFilters) {
      setQuickFilters(JSON.parse(savedFilters));
    }
  }, []);

  return (
    <CurrencyContext.Provider
      value={{
        currencies,
        currency,
        amount,
        exchangeRate,
        loading,
        error,
        showDetails,
        currencyDetails,
        setCurrency,
        setAmount,
        setError,
        toggleDetails,
        quickFilters,
        addQuickFilter,
        removeQuickFilter,
        applyQuickFilter
      }}
    >
      {children}
    </CurrencyContext.Provider>
  );
};

export default CurrencyProvider;
