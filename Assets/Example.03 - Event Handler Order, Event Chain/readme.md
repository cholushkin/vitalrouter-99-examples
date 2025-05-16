# VitalRouter Subscriber Ordering

VitalRouter **doesn't** provide mechanisms to control the execution order of subscribers for the same command. It doesn't implement a traditional priority queue system with arbitrary priority levels, but offers control over the concurrency and sequencing of asynchronous handlers.

## Prioritizing a Chain of Commands

1.  **Explicit Publishing Order:** The most straightforward way is to simply publish commands in the desired sequence. If Command B should only happen after Command A, you publish A, wait for its completion (if necessary), and then publish B.
2.  **Command Data and Logic:** Design your commands and handlers such that the data carried by a command or the logic within a handler determines the next command to be published or the next step in a process.
3.  **Middleware/Interceptors:** Use VitalRouter's filter pipeline to inspect commands and their context. Middleware can decide whether to allow a command to proceed to its handlers, publish subsequent commands, or alter the flow based on custom prioritization rules.
4.  **Workflow Management:** For complex sequences, consider implementing a dedicated workflow or state machine that dictates the order and conditions under which commands are published. This logic would reside outside of the individual command handlers and use VitalRouter as the communication layer between steps.

## Core Pub/Sub Principle 

The publisher-subscriber pattern aims for maximum loose coupling. Publishers emit events without any knowledge of who the subscribers are, how many there are, or what they do. Subscribers listen for events they care about without needing to know about other subscribers.