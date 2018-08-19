using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.ComponentModel;
using System.Threading;
using SlackAPI;

namespace Slack.Activities
{

    public sealed class PostToChannel : CodeActivity
    {
        [Category("Input")]
        [DisplayName("Mesage")]
        [RequiredArgument]
        public InArgument<String> Message { get; set; }

        [Category("Input")]
        [DisplayName("Channel Name")]
        [RequiredArgument]
        public InArgument<string> ChannelName { get; set; }

        [Category("Output")]
        public OutArgument<String> Response { get; set; }

        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override void Execute(CodeActivityContext context)
        {
            SlackClient.StartAuth((authStartResponse) => {
                // Here authStartResponse has a field ('users') containing a list of teams you have access to.
                SlackClient.AuthSignin(
                    (authSigninResponse) => {
            //Here, authSigninResponse contains a field 'token' which is your valid authentication token.
        },
                    authStartResponse.users[0].user_id,
                    authStartResponse.users[1].team_id,
                    "220496"
                );
            }, "dinesh.b@fuzzy15s.com");

            ManualResetEventSlim clientReady = new ManualResetEventSlim(false);
            SlackSocketClient client = new SlackSocketClient(YOUR_AUTH_TOKEN);
            client.Connect((connected) => {
                // This is called once the client has emitted the RTM start command
                clientReady.Set();
            }, () => {
                // This is called once the RTM client has connected to the end point
            });
            client.OnMessageReceived += (message) =>
            {
                // Handle each message as you receive them
            };
            clientReady.Wait();
        }
    }
}
