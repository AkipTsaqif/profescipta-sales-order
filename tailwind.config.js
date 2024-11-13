/** @type {import('tailwindcss').Config} */
module.exports = {
    mode: 'aot',
    content: [
        './Views/**/*.cshtml',
        './Pages/**/*.cshtml',
        './wwwroot/**/*.js'
    ],
    safelist: [
        "bg-blue-900",
        "hover:bg-blue-700",
        { pattern: /^p-/ },
        { pattern: /^m-/ },
        { pattern: /^w-/ },
    ],
    theme: {
        extend: {},
    },
    plugins: [],
};

