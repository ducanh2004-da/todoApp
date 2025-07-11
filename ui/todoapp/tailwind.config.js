module.exports = {
  mode: 'jit', // Just-In-Time mode for Tailwind CSS
  purge: [
    './src/**/*.{html,ts}', // Adjust the path according to your project structure
  ],
  darkMode: false, // or 'media' or 'class' nếu cần chê độ tối
  theme: {
    extend: {
      // Custom colors and fonts
      colors: {
        primary: '#1d4ed8', // Example primary color
        secondary: '#f59e0b', // Example secondary color
        accent: '#10b981', // Example accent color
      },
      fontFamily: {
        sans: ['Inter', 'sans-serif'], // Example font family
        serif: ['Merriweather', 'serif'], // Example serif font family
      },
    },
  },
  variants: {
    extend: {},
  },
  plugins: [],
}
