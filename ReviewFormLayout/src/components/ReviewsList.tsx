import { useState, useEffect } from "react";

interface Review {
  rating: number;
  name: string;
  comment: string;
  id: string;
  timestamp: number;
}

export const ReviewsList = () => {
  const [reviews, setReviews] = useState<Review[]>([]);

  useEffect(() => {
    const loadReviews = () => {
      const savedReviews = JSON.parse(localStorage.getItem("reviews") || "[]");
      setReviews(savedReviews);
    };

    loadReviews();

    const handleStorageChange = () => {
      loadReviews();
    };

    window.addEventListener("storage", handleStorageChange);
    const interval = setInterval(loadReviews, 1000);

    return () => {
      window.removeEventListener("storage", handleStorageChange);
      clearInterval(interval);
    };
  }, []);

  if (reviews.length === 0) {
    return null;
  }

  return (
    <div>
      <h3 className="mb-4">Отзывы</h3>
      <div className="d-flex flex-column gap-3">
        {reviews
          .sort((a, b) => b.timestamp - a.timestamp)
          .map((review) => (
            <div key={review.id} className="border rounded p-3">
              <div className="d-flex justify-content-between align-items-start">
                <div className="flex-grow-1 me-3">
                  <h5 className="text-start mb-2">{review.name}</h5>
                  {review.comment && (
                    <p className="text-start mb-0 text-muted">
                      {review.comment}
                    </p>
                  )}
                </div>
                <div className="text-end">
                  <span className="h5 text-primary">{review.rating}/5</span>
                </div>
              </div>
            </div>
          ))}
      </div>
    </div>
  );
};
