/** @type {import('tailwindcss').Config} */
module.exports = {
    mode: 'aot',
    content: [
        './Views/**/*.cshtml',
        './Pages/**/*.cshtml',
        './wwwroot/**/*.js'
    ],
    safelist: [
        { pattern: /^p-/ },
        { pattern: /^m-/ },
        { pattern: /^w-/ }
    ],
    theme: {
        extend: {},
    },
    plugins: [],
};

