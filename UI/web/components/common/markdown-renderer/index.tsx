import React from "react";

export function MarkdownRenderer({ content }: { content: string }) {
	const parseMarkdown = (text: string) => {
		const lines = text.split("\n");
		const elements: React.ReactNode[] = [];
		let listItems: string[] = [];
		let inCodeBlock = false;
		let codeBlockContent: string[] = [];
		let key = 0;

		const flushList = () => {
			if (listItems.length > 0) {
				elements.push(
					<ul
						key={`list-${key++}`}
						className="list-disc pl-6 my-2 space-y-1"
					>
						{listItems.map((item, idx) => (
							<li key={idx} className="text-white/90">
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
				elements.push(
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

			while (remaining.length > 0) {
				// Bold (**text**)
				const boldMatch = remaining.match(/^\*\*(.+?)\*\*/);
				if (boldMatch) {
					parts.push(
						<strong
							key={`bold-${partKey++}`}
							className="font-bold text-white"
						>
							{boldMatch[1]}
						</strong>,
					);
					remaining = remaining.slice(boldMatch[0].length);
					continue;
				}

				// Italic (*text*)
				const italicMatch = remaining.match(/^\*(.+?)\*/);
				if (italicMatch) {
					parts.push(
						<em key={`italic-${partKey++}`} className="italic">
							{italicMatch[1]}
						</em>,
					);
					remaining = remaining.slice(italicMatch[0].length);
					continue;
				}

				// Inline code (`code`)
				const codeMatch = remaining.match(/^`(.+?)`/);
				if (codeMatch) {
					parts.push(
						<code
							key={`code-${partKey++}`}
							className="bg-black/30 text-cyan-300 px-1.5 py-0.5 rounded text-sm font-mono"
						>
							{codeMatch[1]}
						</code>,
					);
					remaining = remaining.slice(codeMatch[0].length);
					continue;
				}

				// Small text (<small>text</small>)
				const smallMatch = remaining.match(/^<small>(.+?)<\/small>/);
				if (smallMatch) {
					parts.push(
						<small
							key={`small-${partKey++}`}
							className="text-xs text-white/60"
						>
							{smallMatch[1]}
						</small>,
					);
					remaining = remaining.slice(smallMatch[0].length);
					continue;
				}

				// Regular text
				const nextSpecial = remaining.search(/[\*`]/);
				if (nextSpecial === -1) {
					parts.push(remaining);
					break;
				} else {
					parts.push(remaining.slice(0, nextSpecial));
					remaining = remaining.slice(nextSpecial);
				}
			}

			return parts;
		};

		lines.forEach((line, index) => {
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
			const headerMatch = line.match(/^(#{1,6})\s+(.+)$/);
			if (headerMatch) {
				flushList();
				const level = headerMatch[1].length;
				const text = headerMatch[2];
				const sizes = [
					"text-2xl",
					"text-xl",
					"text-lg",
					"text-base",
					"text-sm",
					"text-sm",
				];

				if (level === 1) {
					elements.push(
						<h1
							key={`header-${key++}`}
							className={`${sizes[0]} font-bold text-white mt-4 mb-2`}
						>
							{parseInline(text)}
						</h1>,
					);
				} else if (level === 2) {
					elements.push(
						<h2
							key={`header-${key++}`}
							className={`${sizes[1]} font-bold text-white mt-4 mb-2`}
						>
							{parseInline(text)}
						</h2>,
					);
				} else if (level === 3) {
					elements.push(
						<h3
							key={`header-${key++}`}
							className={`${sizes[2]} font-bold text-white mt-4 mb-2`}
						>
							{parseInline(text)}
						</h3>,
					);
				} else if (level === 4) {
					elements.push(
						<h4
							key={`header-${key++}`}
							className={`${sizes[3]} font-bold text-white mt-4 mb-2`}
						>
							{parseInline(text)}
						</h4>,
					);
				} else if (level === 5) {
					elements.push(
						<h5
							key={`header-${key++}`}
							className={`${sizes[4]} font-bold text-white mt-4 mb-2`}
						>
							{parseInline(text)}
						</h5>,
					);
				} else {
					elements.push(
						<h6
							key={`header-${key++}`}
							className={`${sizes[5]} font-bold text-white mt-4 mb-2`}
						>
							{parseInline(text)}
						</h6>,
					);
				}
				return;
			}

			// List items
			const listMatch = line.match(/^[\s]*[-*]\s+(.+)$/);
			if (listMatch) {
				listItems.push(listMatch[1]);
				return;
			}

			// Numbered list
			const numberedMatch = line.match(/^[\s]*\d+\.\s+(.+)$/);
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
					elements.push(
						<div key={`space-${key++}`} className="h-2" />,
					);
				}
				return;
			}

			// Regular paragraph
			flushList();
			elements.push(
				<p key={`p-${key++}`} className="text-white/90 my-1">
					{parseInline(line)}
				</p>,
			);
		});

		flushList();
		flushCodeBlock();

		return elements;
	};

	return <div className="markdown-content">{parseMarkdown(content)}</div>;
}
