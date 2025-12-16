## Step 5: Using GitHub Copilot within a pull request

Congratulations! You are finished with coding for this exercise (and VS Code). Now it's time to merge our work. :tada: To wrap up, let's learn about two limited-access Copilot features that can speed up our pull requests!

### 📖 Theory: GitHub Copilot for pull requests

#### Copilot pull request summaries

Typically, you would review your notes and commit messages then summarize them for your pull request description. This may take some time, especially if commit messages are inconsistent or code is not documented well. Fortunately, Copilot can consider all changes in the pull request and provide the important highlights, and with references too!

#### Copilot code review

More eyes on our work is always useful so let's ask Copilot to do a first pass before we do a normal peer review process. Copilot is great at catching common mistakes that are fixed by simple adjustments, but please remember to use it responsibly.

### :keyboard: Activity: Create and manage a PR using the GitHub MCP server

Though there are browser based flows for having copilot create a PR, we'll use the **GitHub MCP server** to create and manage pull requests directly from VS Code using Copilot. This demonstrates how Model Context Protocol servers can extend Copilot's capabilities.

#### Install and connect the GitHub MCP server

1. Open **GitHub Copilot Chat** in VS Code (Ctrl+Shift+I or click the chat icon).

1. Click the **Tools icon** (looks like a wrench and screwdriver) in the chat input area.

1. Click the **MCP icon** (looks like a plug) in search drop down that opens at the top.

1. Click **Browser MCP Server** alternatively you can open the Extensions search and query for `@mcp`.

1. Find GitHub in the list and click the Install button.

1. When prompted for a PAT, either provide one, or simply enter blank to prompt the extension to authenticate with OAuth.

#### Create the pull request using Copilot

Now that the GitHub MCP server is connected, you can use Copilot to create and manage pull requests through natural language prompts:

1. Open **GitHub Copilot Chat** in VS Code (Ctrl+Shift+I or click the chat icon).

1. Use the following prompt to create your pull request:
   ```
   @github create a pull request from branch "accelerate-with-copilot" to "main" with the title "Improve student activity registration system"
   ```

1. (Optional) If you have access to **Copilot pull request summaries**, use this prompt to generate a description:
   ```
   @github generate a summary for the pull request I just created
   ```

1. (Optional) If you have access to **Copilot code review**, request a review:
   ```
   @github request a Copilot review on the open pull request
   ```

1. Once you've reviewed the changes, merge the pull request with this prompt:
   ```
   @github merge the pull request from accelerate-with-copilot to main
   ```

   Nice work! You are all done! :tada:

1. Wait a moment for Mona to check your work, provide feedback, and post a final review of this exercise!
