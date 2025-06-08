// components/CurrencyDetails.tsx
import React from 'react';
import { useCurrencyContext } from '../../context/useCurrencyContext';

const CurrencyDetails: React.FC = () => {
  const { showDetails, currencyDetails } = useCurrencyContext();

  if (!showDetails) return null;

  return (
    <div className="mt-3 p-3 border rounded bg-white">
      <h5 className="mb-3 text-center text-primary">
        <i className="bi bi-currency-exchange me-2"></i>
        Currency Details
      </h5>
      <div className="row g-3">
        {(['payment', 'purchase'] as const).map((type) => {
          const currency = currencyDetails[type];
          if (!currency) return null;

          const badgeClass = type === 'payment' ? 'bg-primary' : 'bg-success';

          return (
            <div key={type} className="col-md-6">
              <div className="p-3 h-100">
                <h6 className="d-flex align-items-center">
                  <span className={`badge ${badgeClass} me-2`}>{currency.code}</span>
                  {currency.name}
                </h6>
                <p className="small text-muted">{currency.description}</p>
                <ul className="list-unstyled small">
                  <li className="mb-1">
                    <strong>Symbol:</strong> {currency.symbol}
                  </li>
                  {currency.minPrice !== undefined && (
                    <li className="mb-1">
                      <strong>Min Price:</strong> {currency.minPrice}
                    </li>
                  )}
                </ul>
              </div>
            </div>
          );
        })}
      </div>
    </div>
  );
};

export default CurrencyDetails;
