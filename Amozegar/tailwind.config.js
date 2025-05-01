/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        "./Views/**/*.cshtml", // یا هرجایی که HTML داری
        "./Areas/**/*.cshtml",
        "./wwwroot/**/*.js"
    ],
    theme: {
        extend: {},
    },
    plugins: [],
}