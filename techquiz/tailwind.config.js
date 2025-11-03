/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{html,ts}",
  ],
  darkMode: 'class', // utile pour switcher clair/sombre
  theme: {
    extend: {
      colors: {
        // Palette principale inspirée CodinGame
        'cg-bg': '#0A0E17',
        'cg-card': '#121826',
        'cg-border': '#1E2535',
        'cg-text': '#E5E7EB',
        'cg-text-dim': '#9CA3AF',
        'cg-accent': '#00E0FF',
        'cg-accent-2': '#FF6B00',
        'cg-success': '#00FFAB',
        'cg-error': '#FF4D4D',
      },
      fontFamily: {
        sans: ['Inter', 'Rubik', 'sans-serif'],
        mono: ['JetBrains Mono', 'Share Tech Mono', 'monospace'],
      },
      boxShadow: {
        'neon-blue': '0 0 10px #00E0FF66',
        'neon-orange': '0 0 10px #FF6B0066',
      },
      backgroundImage: {
        'grid': "radial-gradient(circle at 1px 1px, #1E2535 1px, transparent 0)",
      },
      animation: {
        fadeIn: 'fadeIn 0.6s ease-in-out',
        glow: 'glow 1.5s ease-in-out infinite alternate',
      },
      keyframes: {
        fadeIn: {
          '0%': { opacity: 0 },
          '100%': { opacity: 1 },
        },
        glow: {
          '0%': { textShadow: '0 0 5px #00E0FF, 0 0 10px #00E0FF' },
          '100%': { textShadow: '0 0 15px #00E0FF, 0 0 25px #00E0FF' },
        },
      },
    },
  },
  plugins: [],
}
