import React, { useCallback, useEffect, useState } from 'react';
import { Line } from 'react-chartjs-2';
import {
  Chart as ChartJS,
  LineElement,
  PointElement,
  LinearScale,
  CategoryScale,
  Title,
  Tooltip,
  Legend
} from 'chart.js';

import { CurrencyApiClient } from '../../CurrencyApiClient';
import { CurrencyPrice } from '../../types';

const apiClient = new CurrencyApiClient();

interface CurrencyChartProps {
  paymentCurrency: string;
  purchaseCurrency: string;
}

type TimeInterval = '1 MIN' | '2 MIN' | '3 MIN' | '4 MIN' | '5 MIN';

ChartJS.register(LineElement, PointElement, LinearScale, CategoryScale, Title, Tooltip, Legend);

const CurrencyChart: React.FC<CurrencyChartProps> = ({ paymentCurrency, purchaseCurrency }) => {
  const [data, setData] = useState<CurrencyPrice[]>([]);
  const [timeInterval, setTimeInterval] = useState<TimeInterval>('5 MIN');

  const getMilliseconds = (interval: TimeInterval) => {
    switch (interval) {
      case '5 MIN':
        return 5 * 60 * 1000;
      case '4 MIN':
        return 4 * 60 * 1000;
      case '3 MIN':
        return 3 * 60 * 1000;
      case '2 MIN':
        return 2 * 60 * 1000;
      case '1 MIN':
        return 1 * 60 * 1000;
      default:
        return 5 * 60 * 1000;
    }
  };

  const fetchData = useCallback(async () => {
    try {
      const fromDateTime = new Date(Date.now() - getMilliseconds(timeInterval)).toISOString();
      console.log(fromDateTime);

      const prices = await apiClient.getCurrencyPrices(paymentCurrency, purchaseCurrency, fromDateTime);
      setData(prices);
    } catch (error) {
      console.error('Ошибка при загрузке данных:', error);
      setData([]);
    }
  }, [paymentCurrency, purchaseCurrency, timeInterval]);

  useEffect(() => {
    if (paymentCurrency && purchaseCurrency) {
      fetchData();
    }
  }, [paymentCurrency, purchaseCurrency, timeInterval, fetchData]);

  const chartData = {
    labels: data.map((item) => item.datetime),
    datasets: [
      {
        data: data.map((item) => item.price),
        borderColor: '#3467d5',
        fill: true,
        backgroundColor: 'rgba(52, 103, 213, 0.5)',
        pointRadius: 3,
        tension: 0.1
      }
    ]
  };

  const options = {
    responsive: true,
    scales: {
      x: {
        ticks: {
          display: false
        }
      }
    },
    plugins: {
      legend: {
        display: false
      },
      title: {
        display: false
      }
    }
  };

  return (
    <div>
      <div className="d-flex flex-row align-items-center justify-content-end">
        {(['5 MIN', '4 MIN', '3 MIN', '2 MIN', '1 MIN'] as TimeInterval[]).map((interval) => (
          <button
            className="btn btn-primary m-1 d-flex pointer"
            key={interval}
            onClick={() => setTimeInterval(interval)}
            disabled={timeInterval === interval}
          >
            {interval}
          </button>
        ))}
      </div>
      <Line data={chartData} options={options} />
    </div>
  );
};

export default CurrencyChart;
