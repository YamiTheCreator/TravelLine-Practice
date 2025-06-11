import { create } from "zustand";
import type { WordType as Word } from "../types/types";
import { createJSONStorage, persist } from "zustand/middleware";

type Store = {
  words: Word[];
  addWord: (word: string, translation: string) => void;
  deleteWord: (id: string) => void;
  getWordById: (id: string) => Word | undefined;
  updateWord: (id: string, russian: string, english: string) => void;
};

//приводит строку к виду Xxxxxx без лишних пробелов
const toNormalForm = (s: string) => {
  s = s.trim();
  return s.charAt(0).toUpperCase() + s.slice(1).toLowerCase();
};

const useStore = create<Store>()(
  persist(
    (set, get) => ({
      words: [],

      addWord: (rus, eng) => {
        const { words } = get();
        const newWord: Word = {
          id: crypto.randomUUID(),
          russian: toNormalForm(rus),
          english: toNormalForm(eng),
        };
        set({ words: [...words, newWord] });
      },
      deleteWord: (id) => {
        set({
          words: get().words.filter((word) => word.id !== id),
        });
      },
      getWordById: (id) => {
        const words = get().words;
        return words.find((word) => word.id === id);
      },
      updateWord: (id, russian, english) => {
        set((state) => ({
          words: state.words.map((word) =>
            word.id === id
              ? {
                  ...word,
                  russian: toNormalForm(russian),
                  english: toNormalForm(english),
                }
              : word
          ),
        }));
      },
    }),
    {
      name: "default",
      storage: createJSONStorage(() => localStorage),
    }
  )
);

export default useStore;
