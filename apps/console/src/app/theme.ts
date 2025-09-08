import { theme } from "antd";

/** Concise theme tokens for light/dark. */
export const lightTheme = {
    algorithm: [theme.defaultAlgorithm, theme.compactAlgorithm],
    token: { colorPrimary: "#1677ff", borderRadius: 8 },
};

export const darkTheme = {
    algorithm: [theme.darkAlgorithm, theme.compactAlgorithm],
    token: { colorPrimary: "#1677ff", borderRadius: 8 },
};