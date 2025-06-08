// components/CurrencyExchanger.tsx
import React from 'react';
import { useCurrencyContext } from '../../context/useCurrencyContext';
import CurrencyChart from '../CurrencyChart/CurrencyChart';
import QuickFilters from '../QuickFilters/QuickFilters';
import CurrencyAbout from '../CurrencyAbout/CurrencyAbout';

const CurrencyExchanger: React.FC = () => {
  const { currency, amount, exchangeRate, loading, error, currencies, setCurrency, setAmount } = useCurrencyContext();

  const result = exchangeRate ? amount * exchangeRate.price : 0;

  const paymentCurrency = currencies.find((c) => c.code === currency.paymentCurrency);
  const purchaseCurrency = currencies.find((c) => c.code === currency.purchaseCurrency);

  if (!error) {
    return (
      <div className="card shadow-sm border-0">
        <div className="card-header bg-primary text-white rounded-top">
          <h3 className="mb-0 text-center py-2">Currency Converter</h3>
        </div>
        <div className="card-body p-4">
          <div className="d-flex flex-column justify-content-center align-items-center mb-4">
            {currencies.length > 0 && (
              <>
                <p>
                  {amount} {paymentCurrency?.name} is
                </p>
                <h1>
                  {result.toFixed(2)} {purchaseCurrency?.name}
                </h1>
                <span>
                  {new Intl.DateTimeFormat('en-US', {
                    timeZone: 'UTC',
                    weekday: 'short',
                    day: '2-digit',
                    month: 'short',
                    year: 'numeric',
                    hour: '2-digit',
                    minute: '2-digit',
                    hour12: false
                  }).format(new Date()) + ' UTC'}
                </span>
              </>
            )}
          </div>

          <div className="d-flex justify-content-end">
            <QuickFilters />
          </div>

          <div className="mb-4">
            {(['payment', 'purchase'] as const).map((type) => (
              <div className="mb-4" key={type}>
                <label className="form-label fw-semibold">{type === 'payment' ? 'Amount' : 'Converted Amount'}</label>
                <div className="input-group">
                  {type === 'payment' ? (
                    <input
                      type="number"
                      className="form-control form-control-lg"
                      value={amount}
                      onChange={(e) => setAmount(Math.max(0, Number(e.target.value)))}
                      min="0"
                      step="0.01"
                      disabled={loading.rate}
                    />
                  ) : (
                    <input
                      type="text"
                      className="form-control form-control-lg bg-light"
                      value={error ? '—' : result.toFixed(2)}
                      readOnly
                    />
                  )}
                  <select
                    className="form-select form-select-lg"
                    value={currency[`${type}Currency`]}
                    onChange={(e) =>
                      setCurrency((prev) => ({
                        ...prev,
                        [`${type}Currency`]: e.target.value
                      }))
                    }
                    disabled={loading.rate}
                  >
                    {currencies.map((currency) => (
                      <option key={`${type}-${currency.code}`} value={currency.code}>
                        {currency.code}
                      </option>
                    ))}
                  </select>
                </div>
              </div>
            ))}
          </div>

          <CurrencyChart paymentCurrency={currency.paymentCurrency} purchaseCurrency={currency.purchaseCurrency} />
          <CurrencyAbout />
        </div>
      </div>
    );
  }
};

export default CurrencyExchanger;
