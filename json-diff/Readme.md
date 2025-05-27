# JSON-DIFF

Учебный проект для изучения базовых основ HTML, CSS, JS, Web APIs.

## Полезные ссылки

По ссылкам находится теория, которая применяется в проекте.

### HTML

- [\<html\>](https://doka.guide/html/html/)
- [\<link\>](https://doka.guide/html/link/)
- [Что такое семантика](https://doka.guide/html/semantics/)
- [\<div\>](https://doka.guide/html/div/)
- [\<p\>](https://doka.guide/html/p/)
- [\<span\>](https://doka.guide/html/span/)
- [\<pre\>](https://doka.guide/html/pre/)
- [\<form\>](https://doka.guide/html/form/)
- [\<input\>](https://doka.guide/html/input/)
- [\<textarea\>](https://doka.guide/html/textarea/)
- [\<button\>](https://doka.guide/html/button/)
- [\<label\>](https://doka.guide/html/label/)
- [\<script\>](https://doka.guide/html/script/)

### CSS

- [Блочная модель](https://doka.guide/css/box-model/)
- [Специфичность](https://doka.guide/css/specificity/)
- [Принцип каскада](https://doka.guide/css/cascade/)
- [Наследование](https://doka.guide/css/inheritance/)
- [CSS-правило](https://doka.guide/css/css-rule/)
- [Селекторы по тэгу](https://doka.guide/css/tag-selector/)
- [Селекторы по классу](https://doka.guide/css/class-selector/)
- [Селекторы по идентификатору](https://doka.guide/css/id-selector/)
- [Селектор потомка](https://doka.guide/css/nesting-selector/)
- [Селектор по атрибуту](https://doka.guide/css/attribute-selector/)
- [Комбинированные селекторы](https://doka.guide/css/combined-selectors/)
- [Перечисление селекторов](https://doka.guide/css/selector-list/)
- [Псевдоклассы](https://doka.guide/css/pseudoclasses/)
- [:hover](https://doka.guide/css/hover/)
- [@media](https://doka.guide/css/media/)
- [background-color](https://doka.guide/css/background-color/)
- [font-family](https://doka.guide/css/font-family/)
- [font-size](https://doka.guide/css/font-size/)
- [font-weight](https://doka.guide/css/font-weight/)
- [color](https://doka.guide/css/color/)
- [text-decoration](https://doka.guide/css/text-decoration/)
- [border](https://doka.guide/css/border/)
- [width](https://doka.guide/css/width/)
- [height](https://doka.guide/css/height/)
- [margin](https://doka.guide/css/margin/)
- [padding](https://doka.guide/css/padding/)
- [Гайд по flexbox](https://doka.guide/css/flexbox-guide/)
- [Гайд по grid](https://doka.guide/css/grid-guide/)

### JavaScript

- [Переменные](https://learn.javascript.ru/variables)
- [Типы данных](https://learn.javascript.ru/types)
- [Операторы сравнения](https://learn.javascript.ru/comparison)
- [Условные ветвления](https://learn.javascript.ru/ifelse)
- [Стрелочные функции](https://learn.javascript.ru/arrow-functions-basics)
- [Отладка в браузере](https://learn.javascript.ru/debugging-chrome)
- [Объекты](https://learn.javascript.ru/object)
- [Строки](https://learn.javascript.ru/string)
- [Массивы](https://learn.javascript.ru/array)
- [Map и Set](https://learn.javascript.ru/map-set)
- [Методы массивов](https://learn.javascript.ru/array-methods)
- [Формат JSON](https://learn.javascript.ru/json)
- [Object.keys](https://learn.javascript.ru/keys-values-entries)
- [Деструктурирующее присваивание](https://learn.javascript.ru/destructuring-assignment)
- [Рекурсия и стэк](https://learn.javascript.ru/recursion)
- [Обработка ошибок](https://learn.javascript.ru/try-catch)
- [LocalStorage](https://learn.javascript.ru/localstorage)

### Web APIs

- [Поиск: querySelector*](https://learn.javascript.ru/searching-elements-dom)
- [classList](https://learn.javascript.ru/styles-and-classes#classname-i-classlist)
- [Свойства узлов: тип, тэг, содержимое](https://learn.javascript.ru/basic-dom-node-properties)
- [Введение в браузерное событие](https://learn.javascript.ru/introduction-browser-events)
- [Действия браузера по умолчанию](https://learn.javascript.ru/default-browser-action)
- [Атрибуты и свойства](https://learn.javascript.ru/dom-attributes-and-properties)

## Описание работы приложения

- **Важно:** приложение - одностраничное, это значит, что в приложении должен быть только один html-файл index.html
- В приложении используется "голые" HTML, CSS, JS. Сторонних библиотек не используется
- Код разделен на логические модули (JS и CSS) для удобства разработки и поддержки приложения
- В шапке приложения находятся следующие активные элементы:
  - логотип, 
  - приветствие пользователя (если он авторизован)
  - ссылка на входа в приложение (Log in) или ссылка выход (Log out), зависит от того авторизован пользователь или нет

### Страница promo

- Страница promo - главная страница приложение. Клик по логитипу ведёт на неё.

![страница promo](https://github.com/LehaIvanov/frontend-course/blob/main/json-diff/docs/promo.png)

### Страница promo, когда пользователь залогинен

- когда пользователь залогинен
  - в шапке появляется приветствие пользователя
  - под промо-текстом появляется ссылка Start, которая ведёт на на форму сравнения json-ов

![страница promo для авторизованного пользователя](https://github.com/LehaIvanov/frontend-course/blob/main/json-diff/docs/promo-logged-in.png)

### Форма логинации

- авторизация происходит путём сохранения логина пользователя в localStorage
- если пользователь не ввёл логин и пытается авторизоваться, то выводится сообщение об ошибке

![форма логинации](https://github.com/LehaIvanov/frontend-course/blob/main/json-diff/docs/login.png)

![форма логинации с ошибкой](https://github.com/LehaIvanov/frontend-course/blob/main/json-diff/docs/login-error.png)

### Форма сравнения json-ов

- если пользователь не ввёл json или ввёл невалидный json, то выводится соответствующее сообщение

![форма сравнения json-ов](https://github.com/LehaIvanov/frontend-course/blob/main/json-diff/docs/json-diff.png)
![форма сравнения json-ов с ошибкой](https://github.com/LehaIvanov/frontend-course/blob/main/json-diff/docs/json-diff-error.png)

### Отображение результата сравнения json-ов

![форма сравнения json-ов с результатом](https://github.com/LehaIvanov/frontend-course/blob/main/json-diff/docs/json-diff-result.png)

## Пример расчёта разницы между json-ами

OLD JSON

``` json
{
  "timeout": 20,
  "verbose": true,
  "host": "google.com"
}
```

NEW JSON

``` json
{
  "timeout": 50,
  "proxy": "888.888.88.88",
  "host": "google.com"
}
```

RESULT

``` json
{
  "timeout": {
    "type": "changed",
    "oldValue": 20,
    "newValue": 50
  },
  "verbose": {
    "type": "deleted",
    "oldValue": true
  },
  "host": {
    "type": "unchanged",
    "oldValue": "google.com",
    "newValue": "google.com"
  },
  "proxy": {
    "type": "new",
    "newValue": "888.888.88.88"
  }
}
```
