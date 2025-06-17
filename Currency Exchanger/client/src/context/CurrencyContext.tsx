import { createContext } from 'react';
import { Currency, CurrencyPrice, QuickFilter } from '../types';

interface CurrencyContextType {
  currencies: Currency[];
  currency: { paymentCurrency: string; purchaseCurrency: string };
  amount: number;
  exchangeRate: CurrencyPrice | null;
  loading: { initial: boolean; rate: boolean; details: boolean };
  error: string | null;
  showDetails: boolean;
  currencyDetails: { payment: Currency | null; purchase: Currency | null };
  setCurrency: React.Dispatch<React.SetStateAction<{ paymentCurrency: string; purchaseCurrency: string }>>;
  setAmount: React.Dispatch<React.SetStateAction<number>>;
  setError: React.Dispatch<React.SetStateAction<string | null>>;
  toggleDetails: () => void;
  quickFilters: QuickFilter[];
  addQuickFilter: () => void;
  removeQuickFilter: (id: string) => void;
  applyQuickFilter: (filter: QuickFilter) => void;
}

const CurrencyContext = createContext<CurrencyContextType | undefined>(undefined);

export default CurrencyContext;
