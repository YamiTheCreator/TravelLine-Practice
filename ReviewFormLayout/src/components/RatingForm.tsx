import { ReviewsList } from "./ReviewsList";
import { RatingList } from "./RatingList";

export const RatingForm = () => {
  return (
    <div className="d-flex gap-3 flex-column w-auto">
      <RatingList />
      <ReviewsList />
    </div>
  );
};
