# SCSS Folder Structure

This project follows the **7-1 architecture** for organizing SCSS files. The 7-1 architecture divides styles into 7 folders, each responsible for a different part of the styling process. This approach enhances maintainability and scalability as your project grows.

## Folder Structure:

```
scss/
│
├── abstracts/           # Variables, functions, and mixins
├── base/                # Reset, typography, base styles
├── components/          # Component-specific styles (e.g., buttons, forms, cards)
├── layout/              # Layout-specific styles (e.g., header, footer, grid)
├── pages/               # Styles for specific pages (e.g., index, login, order, signup, table, forgotpassword)
├── themes/              # Optional themes (e.g., light, dark mode)
├── vendors/             # Overrides for third-party libraries (e.g., Bootstrap)
└── main.scss            # Main entry point that imports everything
```

---

## Folder Breakdown:

### 1. `/abstracts/`

This folder contains globally used SCSS variables, mixins, and functions.

- **`_variables.scss`**: Stores global variables like colors, fonts, breakpoints.
- **`_mixins.scss`**: Reusable mixins (e.g., for media queries, flexbox helpers).
- **`_functions.scss`**: Custom SCSS functions to perform calculations or other logic.

### 2. `/base/`

The `/base/` folder holds the foundational styles applied throughout the app.

- **`_reset.scss`**: Contains CSS reset rules (e.g., Normalize or custom reset).
- **`_typography.scss`**: Sets font styles, heading, paragraph styles, and text alignment.
- **`_base.scss`**: Defines basic element styles, such as `body`, `a`, `p`, `ul`, etc.

### 3. `/components/`

Component-specific styles for reusable elements like buttons, forms, and cards.

- **`_button.scss`**: Styles for buttons used across the application.
- **`_form.scss`**: Styles for form inputs, labels, checkboxes, etc.
- **`_tabs.scss`**: Styles for tabs on the **Order** page.

### 4. `/layout/`

The layout folder holds styles related to the overall page layout (e.g., header, footer, grid).

- **`_header.scss`**: Styles for the header or navigation bar.
- **`_footer.scss`**: Styles for the footer section.
- **`_grid.scss`**: Optional grid or layout system styles for structuring the page.
- **`_container.scss`**: General container styles for controlling the layout width and padding.

### 5. `/pages/`

This folder contains styles specific to individual pages.

- **`_index.scss`**: Styles for the home (index) page, typically for the hero section and buttons.
- **`_login.scss`**: Styles for the login page (e.g., form layout, input fields).
- **`_signup.scss`**: Styles for the sign-up page.
- **`_order.scss`**: Styles for the **Order** page, including tabs for menu, order summary, and AI recommendations.
- **`_table.scss`**: Styles for the table selection page.
- **`_forgotpassword.scss`**: Styles for the forgot password page.

### 6. `/themes/`

Optional folder to manage multiple themes (e.g., light mode, dark mode).

- **`_default.scss`**: Default theme styles (e.g., base colors, backgrounds).
- **Additional themes** can be added here if needed.

### 7. `/vendors/`

This folder is for third-party library overrides (e.g., Bootstrap, jQuery UI).

- **`_bootstrap.scss`**: Custom overrides for Bootstrap (if Bootstrap is used in the project).

---

## Main Entry Point: `main.scss`

The **`main.scss`** file is the single entry point for importing all other partial SCSS files. It brings together all the styles into a single compiled CSS file.

```scss
// abstracts
@import "abstracts/variables";
@import "abstracts/mixins";
@import "abstracts/functions";

// base
@import "base/reset";
@import "base/typography";
@import "base/base";

// layout
@import "layout/header";
@import "layout/footer";
@import "layout/grid";
@import "layout/container";

// components
@import "components/button";
@import "components/form";
@import "components/tabs";

// pages
@import "pages/index";
@import "pages/login";
@import "pages/signup";
@import "pages/order";
@import "pages/table";
@import "pages/forgotpassword";

// themes
@import "themes/default";

// vendors
@import "vendors/bootstrap";
```

---

## Compilation

You can compile your SCSS to CSS using any SCSS compiler (e.g., using **Web Compiler** in Visual Studio, **Node.js** with **Sass**, or **Gulp**). The compiled CSS file will typically be placed in `/wwwroot/css/`.

---

## Contribution

When adding new styles:

- Follow the **7-1 architecture** by placing styles in the appropriate folder.
- For new components, create a new SCSS file inside `/components/`.
- For new pages, create a new SCSS file inside `/pages/`.

---