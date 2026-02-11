import React from "react";

export function MarkdownRenderer({ content }: { readonly content: string }) {
	const parseMarkdown = (text: string) => {
		const lines = text.split("\n");
		const elements: React.ReactNode[] = [];
		let listItems: string[] = [];
		let inCodeBlock = false;
		let codeBlockContent: string[] = [];
		let key = 0;

		const pushElement = (element: React.ReactNode) => {
			elements.push(element);
		};

		const flushList = () => {
			if (listItems.length > 0) {
				pushElement(
					<ul
						key={`list-${key++}`}
						className="list-disc pl-6 my-2 space-y-1"
					>
						{listItems.map((item) => (
							<li key={item} className="text-white/90">
								{parseInline(item)}
							</li>
						))}
					</ul>,
				);
				listItems = [];
			}
		};

		const flushCodeBlock = () => {
			if (codeBlockContent.length > 0) {
				pushElement(
					<pre
						key={`code-${key++}`}
						className="bg-black/30 border border-white/10 rounded-lg p-3 my-2 overflow-x-auto"
					>
						<code className="text-cyan-300 text-sm font-mono">
							{codeBlockContent.join("\n")}
						</code>
					</pre>,
				);
				codeBlockContent = [];
			}
		};

		const parseInline = (text: string): React.ReactNode => {
			const parts: React.ReactNode[] = [];
			let remaining = text;
			let partKey = 0;

			const inlinePatterns = [
				{
					regex: /^\*\*([^*]+(?:\*(?!\*)[^*]*)*)\*\*/,
					tag: "strong",
					className: "font-bold text-white",
				},
				{
					regex: /^\*([^*]+(?:\*(?!\*)[^*]*)*)\*/,
					tag: "em",
					className: "italic",
				},
				{
					regex: /^`([^`]+)`/,
					tag: "code",
					className:
						"bg-black/30 text-cyan-300 px-1.5 py-0.5 rounded text-sm font-mono",
				},
				{
					regex: /^<small>([^<]*)<\/small>/,
					tag: "small",
					className: "text-xs text-white/60",
				},
			];

			while (remaining.length > 0) {
				let matched = false;

				for (const pattern of inlinePatterns) {
					const match = pattern.regex.exec(remaining);
					if (match) {
						parts.push(
							React.createElement(
								pattern.tag,
								{
									key: `${pattern.tag}-${partKey++}`,
									className: pattern.className,
								},
								match[1],
							),
						);
						remaining = remaining.slice(match[0].length);
						matched = true;
						break;
					}
				}

				if (!matched) {
					const nextSpecial = remaining.search(/[*`<]/);
					if (nextSpecial === -1) {
						parts.push(remaining);
						break;
					} else {
						parts.push(remaining.slice(0, nextSpecial));
						remaining = remaining.slice(nextSpecial);
					}
				}
			}

			return parts;
		};

		const createHeaderElement = (level: number, text: string) => {
			const sizes = [
				"text-2xl",
				"text-xl",
				"text-lg",
				"text-base",
				"text-sm",
				"text-sm",
			];
			const size = sizes[Math.min(level - 1, 5)];

			return React.createElement(
				`h${level}` as keyof React.JSX.IntrinsicElements,
				{
					key: `header-${key++}`,
					className: `${size} font-bold text-white mt-4 mb-2`,
				},
				parseInline(text),
			);
		};

		const processLine = (line: string, index: number) => {
			// Code block
			if (line.trim().startsWith("```")) {
				if (inCodeBlock) {
					flushCodeBlock();
					inCodeBlock = false;
				} else {
					flushList();
					inCodeBlock = true;
				}

				return;
			}

			if (inCodeBlock) {
				codeBlockContent.push(line);
				return;
			}

			// Headers
			const headerMatch = /^(#{1,6})\s+(.+)$/.exec(line);
			if (headerMatch) {
				flushList();
				pushElement(
					createHeaderElement(headerMatch[1].length, headerMatch[2]),
				);
				return;
			}

			// List items
			const listMatch = /^\s*[-*]\s+(.+)$/.exec(line);
			if (listMatch) {
				listItems.push(listMatch[1]);
				return;
			}

			// Numbered list
			const numberedMatch = /^\s*\d+\.\s+(.+)$/.exec(line);
			if (numberedMatch) {
				flushList();
				if (listItems.length === 0) {
					listItems.push(numberedMatch[1]);
				}
				return;
			}

			// Empty line
			if (line.trim() === "") {
				flushList();
				if (elements.length > 0 && index < lines.length - 1) {
					pushElement(<div key={`space-${key++}`} className="h-2" />);
				}
				return;
			}

			// Regular paragraph
			flushList();
			pushElement(
				<p key={`p-${key++}`} className="text-white/90 my-1">
					{parseInline(line)}
				</p>,
			);
		};

		lines.forEach((item, index) => processLine(item, index));
		flushList();
		flushCodeBlock();

		return elements;
	};

	return <div className="markdown-content">{parseMarkdown(content)}</div>;
}
