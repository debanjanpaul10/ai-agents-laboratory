import type { NextConfig } from "next";
import path from "path";

const nextConfig: NextConfig = {
	/* config options here */
	webpack: (config, { isServer }) => {
		// Add environment alias for webpack
		config.resolve.alias = {
			...config.resolve.alias,
			"@environments": isServer ? "./environments" : "../environments",
		};

		return config;
	},
	turbopack: {
		root: path.join(__dirname, ".."),
	},
};

export default nextConfig;
